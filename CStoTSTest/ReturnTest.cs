using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("Returnのテスト", nameof(ReturnTest))]
  public class ReturnTest : TestBase
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
          return;
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    return;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "IntTest")]
    public void IntTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public int Method()
        {
          return 100;
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): number {");
      expectedTS.AppendLine("    return 100;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "InstanceTest")]
    public void InstanceTest()
    {
      // C#ソース作成
      CreateFileData("myclass.cs", string.Empty,
      @"public class MyClass
      {
      }");
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public MyClass Method()
        {
          return new MyClass();
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { MyClass } from 'myclass';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): MyClass {");
      expectedTS.AppendLine("    return new MyClass();");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  [Fact(DisplayName = "InstanceExistsParamsTest")]
    public void InstanceExistsParamsTest()
    {
      // C#ソース作成
      CreateFileData("myclass.cs", string.Empty,
      @"public class MyClass
      {
        public MyClass(int param1,string param2)
        {
        }
      }");
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public MyClass Method()
        {
          return new MyClass(100,""ABC"");
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { MyClass } from 'myclass';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): MyClass {");
      expectedTS.AppendLine("    return new MyClass(100, \"ABC\");");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "FieldRefarenceTest")]
    public void FieldRefarenceTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private string field1;
        public MyClass Method()
        {
          return field1;
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private field1: string;");
      expectedTS.AppendLine("  public Method(): MyClass {");
      expectedTS.AppendLine("    return this.field1;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }
  }
}
