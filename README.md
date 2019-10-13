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

## TODO
* [X] import(外部参照)のテストと実装を追加
* [X] 階層構造のあるimport(外部参照)のテストと実装を追加
* [X] 固定出力のTypeScriptファイルの格納場所の確定
* [X] JSONシリアライズ/デシリアライズTypeScriptの作成
* [X] プロパティのみ出力フラグの機能追加(I/F用DTOクラス)
* [ ] TypeScript変換処理のテスト作成・実装
   * [X] 列挙型
   * [X] ローカルフィールド
   * [X] ローカルメソッド
   * [X] if-else構文
   * [X] switch構文
   * [X] for構文
   * [ ] foreach構文
   * [ ] while構文
   * [ ] do-while構文


