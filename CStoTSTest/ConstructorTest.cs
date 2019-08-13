using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("コンストラクタのテスト", nameof(ConstructorTest))]
  public class ConstructorTest : TestBase
  {
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public Test()
        {
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  constructor() {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "ExistsParamTest")]
    public void ExistsParamTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public Test(int param1)
        {
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  constructor(param1: number) {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "SubClassTest")]
    public void SubClassTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public Test()
        {
        }
      }");
      CreateFileData("sub.cs", string.Empty,
      @"public class Sub:Test
      {
        public Sub()
        {
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Sub extends Test {");
      expectedTS.AppendLine("  constructor() {");
      expectedTS.AppendLine("    base();");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "SubClassExistsParamTest")]
    public void SubClassExistsParamTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public Test(int param1)
        {
        }
      }");
      CreateFileData("sub.cs", string.Empty,
      @"public class Sub:Test
      {
        public Sub():base(1)
        {
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Sub extends Test {");
      expectedTS.AppendLine("  constructor() {");
      expectedTS.AppendLine("    base(1);");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "InnerClassTest")]
    public void InnerClassTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public Test()
        {
        }
        public class Inner
        {
          public Inner()
          {
          }
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  constructor() {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public static Inner = class {");
      expectedTS.AppendLine("    constructor() {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "OverloadTest")]
    public void OverloadTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public Test()
        {
        }
        public Test(int param1)
        {
        }
        public Test(string param1)
        {
        }
        public Test(int param1,string param2)
        {
        }
        public Test(string param1,string param2,string param3)
        {
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private constructor1() {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private constructor2(param1: number) {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private constructor3(param1: string) {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private constructor4(param1: number, param2: string) {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private constructor5(param1: string, param2: string, param3: string) {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  constructor(param1?: number | string, param2?: string, param3?: string) {");
      expectedTS.AppendLine("    if (param1 === undefined && param2 === undefined && param3 === undefined) {");
      expectedTS.AppendLine("      this.constructor1();");
      expectedTS.AppendLine("      return;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof param1 === 'number' && param2 === undefined && param3 === undefined) {");
      expectedTS.AppendLine("      this.constructor2(param1);");
      expectedTS.AppendLine("      return;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof param1 === 'string' && param2 === undefined && param3 === undefined) {");
      expectedTS.AppendLine("      this.constructor3(param1);");
      expectedTS.AppendLine("      return;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof param1 === 'number' && typeof param2 === 'string' && param3 === undefined) {");
      expectedTS.AppendLine("      this.constructor4(param1, param2);");
      expectedTS.AppendLine("      return;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof param1 === 'string' && typeof param2 === 'string' && typeof param3 === 'string') {");
      expectedTS.AppendLine("      this.constructor5(param1, param2, param3);");
      expectedTS.AppendLine("      return;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}
