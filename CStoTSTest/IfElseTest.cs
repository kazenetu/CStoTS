using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("条件分岐のテスト", nameof(IfElseTest))]
  public class IfElseTest : TestBase
  {
    /// <summary>
    /// 標準テスト
    /// </summary>
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          if(1 == 1){
          }

          if(10 > 10){
          }
          else{
          }

          if(10 >= 10){
          }
          else{
            if(20 < 20){
            }
          }

          if(10 >= 10){
          }
          else if(20 < 20){
          }
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    if (1 === 1) {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (10 > 10) {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    else {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (10 >= 10) {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    else {");
      expectedTS.AppendLine("      if (20 < 20) {");
      expectedTS.AppendLine("      }");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (10 >= 10) {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    else if (20 < 20) {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// インスタンスフィールドとプロパティのテスト
    /// </summary>
    [Fact(DisplayName = "MemberTest")]
    public void MemberTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private string Field = ""abc"";
        private string Prop{get;} = ""123"";

        public void Method(){
          if(Field == ""abc""){
          }
          if(Prop == ""123""){
          }
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private Field: string = \"abc\";");
      expectedTS.AppendLine("  private _Prop_: string = \"123\";");
      expectedTS.AppendLine("  private get Prop(): string {");
      expectedTS.AppendLine("    return this._Prop_;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    if (this.Field === \"abc\") {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (this.Prop === \"123\") {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// Type確認のテスト
    /// </summary>
    [Fact(DisplayName = "TypeTest")]
    public void TypeTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private enum CompassDirection
        {
          North,
          East,
          South,
          West
        }
        private string Field1;
        private int Field2;
        private DateTime Field3;

        public void Method(){
          var compassDirection = CompassDirection.North;
          if(Field1 is string){
          }
          if(Field2 is int){
          }
          if(Field3 is DateTime){
          }
          if(compassDirection is CompassDirection){
          }
        }
      }");
      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private Field1: string;");
      expectedTS.AppendLine("  private Field2: number;");
      expectedTS.AppendLine("  private Field3: Date;");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let compassDirection: Test.CompassDirection = Test.CompassDirection.North;");
      expectedTS.AppendLine("    if (typeof this.Field1 === \"string\") {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof this.Field2 === \"number\") {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (this.Field3 instanceof Date) {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof compassDirection === \"function\") {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");
      expectedTS.AppendLine("export namespace Test {");
      expectedTS.AppendLine("  export enum CompassDirection {");
      expectedTS.AppendLine("    North,");
      expectedTS.AppendLine("    East,");
      expectedTS.AppendLine("    South,");
      expectedTS.AppendLine("    West,");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// 列挙型のテスト
    /// </summary>
    [Fact(DisplayName = "EnumTest")]
    public void EnumTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private enum CompassDirection
        {
          North,
          East,
          South,
          West
        }

        public void Method(){
          var compassDirection = CompassDirection.North;
          if(compassDirection == CompassDirection.North){
          }
        }
      }");
      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let compassDirection: Test.CompassDirection = Test.CompassDirection.North;");
      expectedTS.AppendLine("    if (compassDirection === Test.CompassDirection.North) {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");
      expectedTS.AppendLine("export namespace Test {");
      expectedTS.AppendLine("  export enum CompassDirection {");
      expectedTS.AppendLine("    North,");
      expectedTS.AppendLine("    East,");
      expectedTS.AppendLine("    South,");
      expectedTS.AppendLine("    West,");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// メソッド呼び出しのテスト
    /// </summary>
    [Fact(DisplayName = "CallMethodTest")]
    public void CallMethodTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          if(TestMethod() == ""123""){
          }
          if(TestStaticMethod() == ""123""){
          }
        }
        private string TestMethod(){
          return ""123"";
        }
        private static string TestStaticMethod(){
          return ""456"";
        }
      }");
      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    if (this.TestMethod() === \"123\") {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (Test.TestStaticMethod() === \"123\") {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private TestMethod(): string {");
      expectedTS.AppendLine("    return \"123\";");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private static TestStaticMethod(): string {");
      expectedTS.AppendLine("    return \"456\";");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}
