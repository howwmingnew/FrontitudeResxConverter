<p align="right">
🌐 언어:
<a href="README.md">English</a> |
<a href="README.zh-TW.md">繁體中文</a> |
<a href="README.ko.md">한국어</a>
</p>

# FrontitudeResxConverter

**Frontitude**에서 내보낸 다국어 JSON 파일을  
Visual Studio 확장인 **ResX Manager**에서 가져올 수 있는 Excel `.xlsx` 형식으로 변환하는 작은 유틸리티입니다.

## 개요

이 도구는 다음과 같은 WPF / .NET 기반 애플리케이션을 위해 설계되었습니다.

- UI 텍스트와 번역 관리를 위해 **Frontitude**를 사용하고,
- Visual Studio에서 `.resx` 리소스 파일 관리를 위해 **ResX Manager**를 사용하며,
- Frontitude JSON 내보내기 형식을 ResX Manager의 Excel 가져오기 형식으로 변환해야 하는 경우.

프로그램은 다음을 수행합니다.

- Frontitude JSON 내보내기(언어 → key → value 구조)를 읽고,
- ResX Manager가 기대하는 Excel 레이아웃을 생성합니다.
  - 각 **행(row)** 은 하나의 리소스 키(예: `about_0`)를 나타냅니다.
  - 각 **열(column)** 은 하나의 언어(예: `en_US`, `ar`, `fr`, `zh-CN`, `zh-TW` 등)를 나타냅니다.
  - `Project`, `File`, `Key`, `Comment` 등의 열을 포함하며, 기존 샘플 파일 구조에 맞출 수 있습니다.

## 요구 사항

- .NET SDK (권장: .NET 6 또는 .NET 8)
- Windows 환경 (`windows-latest` GitHub Actions 러너 사용)
- NuGet 패키지: [`ClosedXML`](https://www.nuget.org/packages/ClosedXML) – `.xlsx` 파일 생성을 위해 사용

## Frontitude JSON 형식

예상하는 JSON 구조는 다음과 같습니다.

```jsonc
{
  "en_US": {
    "about_0": "All right reserved.",
    "about_1": "Terms of use"
  },
  "ar": {
    "about_0": "...",
    "about_1": "..."
  },
  "fr": {
    "about_0": "..."
  },
  "zh_CN": {
    "about_0": "..."
  },
  "zh_TW": {
    "about_0": "..."
  }
}
```

- 최상위 key: 언어 코드 (`en_US`, `ar`, `fr`, `zh_CN`, `zh_TW` 등)
- 두 번째 key: 리소스 키 (`about_0` 등), value는 해당 언어의 번역 문자열입니다.

## 출력 Excel 형식 (ResX Manager)

프로그램은 `ResXResourceManager`라는 워크시트를 생성하며,  
ResX Manager의 Excel 가져오기 형식과 호환됩니다.

주요 컬럼 예:

- `Project`
- `File`
- `Key`
- `Comment`
- 기본 언어 컬럼 (예: `en_US`)
- 각 언어 컬럼: `.ar`, `.fr`, `.ja`, `.kk`, `.ko`, `.pl`, `.pt`, `.ro`, `.ru`, `.th`, `.tr`, `.vi`, `.zh-CN`, `.zh-TW` …

실제 헤더 이름과 순서는 사용 중인 ResX Manager 샘플 파일에 맞게 조정할 수 있습니다.

## 사용 방법 (명령줄)

빌드 후, 명령줄에서 다음과 같이 실행합니다.

```bash
FrontitudeToResxXlsx.exe <inputJsonPath> <outputXlsxPath>
```

예:

```bash
FrontitudeToResxXlsx.exe Frontitude_export.json output.xlsx
```

- `inputJsonPath` – Frontitude에서 내보낸 JSON 파일 경로
- `outputXlsxPath` – 생성될 Excel 파일 경로

실행이 완료되면 ResX Manager에서 가져올 수 있는 Excel 파일이 생성됩니다.

## 프로젝트 구조

단순화한 구조 예시는 다음과 같습니다.

```text
FrontitudeResxConverter/
├─ FrontitudeToResxXlsx/          # 콘솔 프로젝트
│  ├─ Core/
│  ├─ Properties/
│  ├─ TestData/
│  ├─ FrontitudeToResxXlsx.csproj
│  └─ FrontitudeToResxXlsx.sln
├─ .github/
│  └─ workflows/
│     └─ release.yml              # GitHub Actions: 자동 빌드 및 릴리스
├─ README.md                      # 영어
├─ README.zh-TW.md                # 번체 중국어
└─ README.ko.md                   # 한국어
```

## 개발

1. Visual Studio 또는 VS Code에서 솔루션을 엽니다.
2. NuGet에서 `ClosedXML` 패키지를 설치합니다.
3. 솔루션을 빌드합니다.
4. 콘솔 앱을 실행하면서 JSON 입력 경로와 Excel 출력 경로를 지정합니다.

## GitHub Actions: 자동 빌드 & 릴리스

저장소에는 `.github/workflows/release.yml` 워크플로우가 포함되어 있습니다.

- 트리거: `v*` 패턴과 일치하는 **태그가 push 될 때** (예: `v1.0.0`)
- `windows-latest` 러너에서 실행:
  - NuGet 패키지 복원
  - Release 구성으로 콘솔 앱 빌드
  - `win-x64` 대상으로 `dotnet publish` 실행, 단일 `.exe` 생성
  - `.zip` 패키지 생성
  - `softprops/action-gh-release`를 사용해 GitHub Release를 생성/업데이트하고:
    - 단일 exe
    - zip 파일
    를 업로드

### 릴리스 흐름

1. 코드를 수정하고 `main` 브랜치에 push 합니다.
2. 태그를 생성합니다.

   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

3. GitHub Actions가 자동으로 실행됩니다.
4. **Releases** 페이지에서 해당 버전의 파일을 다운로드할 수 있습니다.