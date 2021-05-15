# CStoTS
C#をTypeScriptに変換するツール  

<table>
<tr>
<td>
C#
</td>
<td>
</td>
<td>
TypeScript
</td>
</tr>
<tr>
<td>

```Csharp
public class TestClass
{
  public class InnerClass
  {
    public static string StaticField = "789";

    public static string StaticMethod()
    {
      return "aaa";
    }
  }
  public static string StaticField = "456";
  public static string StaticMethod()
  {
    return "bbb";
  }
}
```  

</td>
<td>
⇒
</td>
<td>

```TypeScript
export class TestClass {
  public static StaticField: string = "456";
  public static StaticMethod(): string {
    return "bbb";
  }
}
export namespace TestClass {
  export class InnerClass {
    public static StaticField: string = "789";
    public static StaticMethod(): string {
      return "aaa";
    }
  }
}
```

</td>
</tr>
</table>

## 実行環境
* .NET Core SDK 2.1以上

## ビルド方法
1. git clone
   ```sh
   git clone --recursive https://github.com/kazenetu/CStoTS.git
   ```
1. publish  
   ※**-f**にインストール済みの「netcoreapp2.1」「netcoreapp3.1」「net5.0」いずれかのSDKを指定すること
   ```sh
   cd CStoTS

   # CStoTS/publishにdllなどのファイルが出力される
   dotnet publish ConvertCStoTS -c Release -o publish -f [netcoreapp2.1 | netcoreapp3.1 | net5.0]
   ```

## 使用方法
## ```dotnet publish/ConvertCStoTS.dll <SourcePath> [options]```  

### ```<SourcePath>``` C#ファイルのベースディレクトリ

### ```[options]```
|コマンド          | ファイルパス      |備考|
|:----------------|:-----------------|:-------------|  
|```-f, --file``` | ```<C#ファイルパス>```    |SourcePath以降のC#ファイルまでのパス<br>単体のCSファイルだけ変換する場合に利用|
|```--o, --out``` | ```<TSファイル出力パス>```|TypeScriptを出力する起点ディレクトリ|
|```--no_method_output``` |  |コンストラクタ・メソッドは出力対象外|
|```--h, --help```|                         | ヘルプページを表示する|

## 実行例

### C#ファイルのTypeScript一括変換
* C#ファイルのベースディレクトリ:**Example/Sources**
* TypeScriptを出力する起点ディレクトリ:**Example/ConvertResults/All**
* コンストラクタ・メソッド出力:行う

```dotnet publish/ConvertCStoTS.dll Example/Sources --out Example/ConvertResults/All```  
または  
```dotnet publish/ConvertCStoTS.dll Example/Sources -o Example/ConvertResults/All```

### 単体C#ファイルのTypeScript変換
* C#ファイルのベースディレクトリ:**Example/Sources**
* 対象C#ファイル:**TestMethod.cs**
* TypeScriptを出力する起点ディレクトリ:**Example/ConvertResults/File**
* コンストラクタ・メソッド出力:行う

```dotnet publish/ConvertCStoTS.dll Example/Sources --file TestMethod.cs --out Example/ConvertResults/File```  
または  
```dotnet publish/ConvertCStoTS.dll Example/Sources -f TestMethod.cs --o Example/ConvertResults/File```

### 単体C#ファイルのTypeScript変換：プロパティのみ
* C#ファイルのベースディレクトリ:**Example/Sources**
* 対象C#ファイル:**TestMethod.cs**
* TypeScriptを出力する起点ディレクトリ:**Example/ConvertResults/File**
* コンストラクタ・メソッド出力:**行わない**

```dotnet publish/ConvertCStoTS.dll Example/Sources --file TestMethod.cs --out Example/ConvertResults/WithoutMethod --no_method_output```  
または  
```dotnet publish/ConvertCStoTS.dll Example/Sources -f TestMethod.cs -o Example/ConvertResults/WithoutMethod --no_method_output```


## テスト方法
* VisualStudio(2017以上)を利用する場合  
  ```CStoTSTest.sln```を開いてテストを行う

* dotnetコマンドを利用する場合
  ```sh
  #CStoTSTest/CStoTSTest.csprojのテストを実施
  dotnet test ./CStoTSTest/CStoTSTest.csproj
  ```

## ライセンス
[MIT ライセンス](LICENSE)

## 使用ライブラリ
* [kazenetu/CSharpAnalyze](https://github.com/kazenetu/CSharpAnalyze) ([MIT](https://github.com/kazenetu/CSharpAnalyze/blob/master/LICENSE))
