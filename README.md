# ClipSaver

클립보드에 복사된 이미지를 자동으로 저장하는 트레이 앱

## 필요 환경
- Windows 10/11
- .NET 8.0 SDK (https://dotnet.microsoft.com/download)

## 실행 방법

```bash
# 빌드
dotnet build

# 실행
dotnet run

# 배포용 단일 exe 빌드
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## 사용법

1. 실행하면 트레이 아이콘으로 상주
2. 어디서든 이미지를 복사(Ctrl+C)하면 자동 저장
3. 트레이 아이콘 우클릭 → 설정에서 저장 폴더 변경 가능
4. 트레이 아이콘 더블클릭으로도 설정 열기 가능

## 기본 저장 경로
`내 문서/사진/ClipSaver/`

## 파일 구조

```
ClipSaver/
├── Program.cs              # 진입점
├── TrayApplicationContext.cs  # 트레이 아이콘 & 앱 생명주기
├── ClipboardMonitor.cs     # 클립보드 감지 & 이미지 저장
├── SettingsForm.cs         # 설정 UI
├── AppSettings.cs          # 설정 저장/불러오기
└── ClipSaver.csproj        # 프로젝트 파일
```

## 향후 추가할 수 있는 기능
- [ ] 날짜별 폴더 자동 분류
- [ ] 저장 형식 선택 (PNG/JPEG)
- [ ] 전역 단축키로 저장 폴더 즉시 열기
- [ ] 저장된 이미지 수 통계
- [ ] 특정 시간대만 감지 모드
