using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("ローカルフィールドのテスト", nameof(LocalDeclarationTest))]
  public class LocalDeclarationTest : TestBase
  {
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method()
        {
          var localVar = ""abc"";
          string localString1 = ""DEF"";
          string localString2;
          int localInt1 = 0;
          int localInt2;
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
      expectedTS.AppendLine("    let localVar: string = \"abc\";");
      expectedTS.AppendLine("    let localString1: string = \"DEF\";");
      expectedTS.AppendLine("    let localString2: string;");
      expectedTS.AppendLine("    let localInt1: number = 0;");
      expectedTS.AppendLine("    let localInt2: number;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "DeclarationClassTest")]
    public void DeclarationClassTest()
    {
      // C#ソース作成
      CreateFileData("myclass.cs", string.Empty,
      @"public class MyClass
      {
        public class Inner {
        }
      }");

      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method()
        {
          var localMyClass1 = new MyClass();
          MyClass localMyClass2 = new MyClass();
          MyClass localMyClass3;

          var localInner1 = new MyClass.Inner();
          MyClass.Inner localInner2 = new MyClass.Inner();
          MyClass.Inner localInner3;
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { MyClass } from './myclass';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let localMyClass1: MyClass = new MyClass();");
      expectedTS.AppendLine("    let localMyClass2: MyClass = new MyClass();");
      expectedTS.AppendLine("    let localMyClass3: MyClass;");
      expectedTS.AppendLine("    let localInner1: MyClass.Inner = new MyClass.Inner();");
      expectedTS.AppendLine("    let localInner2: MyClass.Inner = new MyClass.Inner();");
      expectedTS.AppendLine("    let localInner3: MyClass.Inner;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "DeclarationEnumTest")]
    public void DeclarationEnumTest()
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

        public void Method()
        {
          var localVar = CompassDirection.East;
          CompassDirection localEnum1 = CompassDirection.North;
          CompassDirection localEnum2;
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
      expectedTS.AppendLine("    let localVar: Test.CompassDirection = Test.CompassDirection.East;");
      expectedTS.AppendLine("    let localEnum1: Test.CompassDirection = Test.CompassDirection.North;");
      expectedTS.AppendLine("    let localEnum2: Test.CompassDirection;");
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

    [Fact(DisplayName = "DefaultMethodValueTest")]
    public void DefaultMethodValueTest()
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

        public void Method()
        {
          var localVar = ReturnMethod(""abc"");
          string localString = ReturnMethod(""DEF"");
          var localInt1 = ReturnMethod(10);
          int localInt2 = ReturnMethod(100);

          var localEnum = ReturnEnumMethod(CompassDirection.South);
        }

        public string ReturnMethod(string value)
        {
          return value;
        }

        public int ReturnMethod(int value)
        {
          return value;
        }

        public CompassDirection ReturnEnumMethod(CompassDirection value)
        {
          return CompassDirection.North;
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
      expectedTS.AppendLine("    let localVar: string = this.ReturnMethod(\"abc\");");
      expectedTS.AppendLine("    let localString: string = this.ReturnMethod(\"DEF\");");
      expectedTS.AppendLine("    let localInt1: number = this.ReturnMethod(10);");
      expectedTS.AppendLine("    let localInt2: number = this.ReturnMethod(100);");
      expectedTS.AppendLine("    let localEnum: Test.CompassDirection = this.ReturnEnumMethod(Test.CompassDirection.South);");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private ReturnMethod1(value: string): string {");
      expectedTS.AppendLine("    return value;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private ReturnMethod2(value: number): number {");
      expectedTS.AppendLine("    return value;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public ReturnMethod(param1: string | number): any {");
      expectedTS.AppendLine("    if (typeof param1 === 'string') {");
      expectedTS.AppendLine("      return this.ReturnMethod1(param1);");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof param1 === 'number') {");
      expectedTS.AppendLine("      return this.ReturnMethod2(param1);");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public ReturnEnumMethod(value: Test.CompassDirection): Test.CompassDirection {");
      expectedTS.AppendLine("    return Test.CompassDirection.North;");
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

  }
}
