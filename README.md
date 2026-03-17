# ClipSaver

클립보드에 복사된 이미지를 자동으로 저장하는 트레이 앱
어차피 스크린샷 폴더에 들어간다고요? 그러게...

근데 우클릭 이미지 복사도 무지성으로 저장함

## 사용법

1. 실행하면 트레이 아이콘으로 상주 (아직 알림 없음)
2. 이미지를 복사(Ctrl+C)하면 자동 저장
3. 트레이 아이콘 클릭 → 설정에서 저장 폴더 변경 가능

## 기본 저장 경로

`내 문서/사진/ClipSaver/`

## 파일 구조

```
ClipSaver/
├── Program.cs              # 진입점
├── TrayApplicationContext.cs  # 앱 생명주기
├── ClipboardMonitor.cs     # 클립보드 감지 & 이미지 저장
├── SettingsForm.cs         # 설정 UI
├── AppSettings.cs          # 설정 저장/불러오기
└── ClipSaver.csproj        # 프로젝트 파일
```
