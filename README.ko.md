<p align="right">
🌐 언어:
<a href="README.md">English</a> |
<a href="README.zh-TW.md">繁體中文</a> |
<a href="README.ko.md">한국어</a>
</p>

# FrontitudeResxConverter

FrontitudeResxConverter는 **Frontitude에서 내보낸 다국어 JSON**을  
**ResX Manager(Visual Studio 확장)**에서 가져올 수 있는 Excel `.xlsx` 파일로 변환하는 유틸리티입니다.

이 도구는 Frontitude 기반 번역 워크플로우를 ResX 기반 WPF/.NET 프로젝트와 연결하기 위해 만들어졌습니다.

---

## 기능

- Frontitude JSON → ResX Manager Excel 변환
- 번역 컬럼 자동 생성 및 서식 조정
- 한국어 코드 자동 처리:
  - `ko`, `ko-KR`, `ko_KR` → 모두 `.ko` 컬럼으로 매핑
- 출력 파일명을 입력하지 않으면 yyyyMMdd.xlsx 자동 생성
- 인수가 없으면 사용자 입력을 받는 인터랙티브 모드
- **드래그 앤 드롭 실행 지원** (JSON을 exe에 드래그)

---

## Frontitude JSON 형식

```json
{
  "en_US": {
    "about_0": "All rights reserved.",
    "about_1": "Terms of use"
  },
  "ko-KR": {
    "about_0": "..."
  },
  "zh_TW": {
    "about_0": "..."
  }
}
```

---

## Excel 출력 형식 (ResX Manager)

워크시트 이름: `ResXResourceManager`

주요 컬럼:

- Project, File, Key, Comment
- en_US(주 언어)
- `.ar`, `.fr`, `.ja`, `.ko`, `.pl`, `.ru`
- `.zh-CN`, `.zh-TW`
- 기타 언어 매핑

---

# 사용 방법

### 방법 1 — 입력 + 출력 직접 지정
```
FrontitudeToResxXlsx.exe <inputJsonPath> <outputXlsxPath>
```

### 방법 2 — 출력 자동 생성
```
FrontitudeToResxXlsx.exe input.json
```

### 방법 3 — 인수 없이 실행 시 입력 대기
```
FrontitudeToResxXlsx.exe
```

### 방법 4 — 드래그 앤 드롭 실행
JSON 파일을 exe 위에 드래그하면 자동 실행됩니다.

---

## 개발 정보

- .NET 8  
- ClosedXML 사용  
- VS 및 VS Code 지원  

---

## GitHub Actions (자동 빌드 · 릴리스)

- 태그 push 시 자동 빌드 및 릴리스 생성
