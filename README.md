# CStoTS
C#をTypeScriptに変換

## 依存ライブラリ
* [kazenetu/CSharpAnalyze](https://github.com/kazenetu/CSharpAnalyze)

## clone方法
```sh
git clone --recursive https://github.com/kazenetu/CStoTS.git
```

## pull方法
依存ライブラリが更新している可能性があるため下記の方法でpullする  
```sh
git pull --recurse-submodules
```

## テスト方法
* VisualStudio(2017以上)を利用する場合  
  ```CStoTSTest.sln```を開いてテストを行う
* dotnetコマンドを利用する場合
  ```sh
  #CStoTSTest/CStoTSTest.csprojのテストを実施
  dotnet test ./CStoTSTest/CStoTSTest.csproj
  ```
