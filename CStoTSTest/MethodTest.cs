using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("メソッドのテスト", nameof(MethodTest))]
  public class MethodTest : TestBase
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
        public void Method()
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
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// Privateメソッドのテスト
    /// </summary>
    [Fact(DisplayName = "PrivateMethodTest")]
    public void PrivateMethodTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private void Method()
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
      expectedTS.AppendLine("  private Method(): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// Protectedメソッドのテスト
    /// </summary>
    [Fact(DisplayName = "ProtectedMethodTest")]
    public void ProtectedMethodTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        protected void Method()
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
      expectedTS.AppendLine("  protected Method(): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// パラメータありのテスト
    /// </summary>
    [Fact(DisplayName = "ExistsParamTest")]
    public void ExistsParamTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(int param1)
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
      expectedTS.AppendLine("  public Method(param1: number): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// サブクラスのオーバーライドのテスト
    /// </summary>
    [Fact(DisplayName = "SubClassTest")]
    public void SubClassTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public virtual void Method()
        {
        }
      }");
      CreateFileData("sub.cs", string.Empty,
      @"public class Sub:Test
      {
        public override void Method()
        {
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test } from './test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub extends Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// 内部クラスのメソッド定義のテスト
    /// </summary>
    [Fact(DisplayName = "InnerClassTest")]
    public void InnerClassTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public class Inner
        {
          public void Method()
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
      expectedTS.AppendLine("}");

      expectedTS.AppendLine("export namespace Test {");
      expectedTS.AppendLine("  export class Inner {");
      expectedTS.AppendLine("    public Method(): void {");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// オーバーロードのテスト
    /// </summary>
    [Fact(DisplayName = "OverloadTest")]
    public void OverloadTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method()
        {
        }
        public void Method(int param1)
        {
        }
        public void Method(string param1)
        {
        }
        public void Method(int param1,string param2)
        {
        }
        public void Method(string param1,string param2,string param3)
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
      expectedTS.AppendLine("  private Method1(): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private Method2(param1: number): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private Method3(param1: string): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private Method4(param1: number, param2: string): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  private Method5(param1: string, param2: string, param3: string): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("  public Method(param1?: number | string, param2?: string, param3?: string): void {");
      expectedTS.AppendLine("    if (param1 === undefined && param2 === undefined && param3 === undefined) {");
      expectedTS.AppendLine("      this.Method1();");
      expectedTS.AppendLine("      return;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof param1 === 'number' && param2 === undefined && param3 === undefined) {");
      expectedTS.AppendLine("      this.Method2(param1);");
      expectedTS.AppendLine("      return;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof param1 === 'string' && param2 === undefined && param3 === undefined) {");
      expectedTS.AppendLine("      this.Method3(param1);");
      expectedTS.AppendLine("      return;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof param1 === 'number' && typeof param2 === 'string' && param3 === undefined) {");
      expectedTS.AppendLine("      this.Method4(param1, param2);");
      expectedTS.AppendLine("      return;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    if (typeof param1 === 'string' && typeof param2 === 'string' && typeof param3 === 'string') {");
      expectedTS.AppendLine("      this.Method5(param1, param2, param3);");
      expectedTS.AppendLine("      return;");
      expectedTS.AppendLine("    }");
      expectedTS.AppendLine("    throw new Error('Error');");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// ジェネリクスのテスト
    /// </summary>
    [Fact(DisplayName = "GenericsTest")]
    public void GenericsTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method<T>(T param1)
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
      expectedTS.AppendLine("  public Method<T>(param1: T): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// クラスメソッドのテスト
    /// </summary>
    [Fact(DisplayName = "StaticTest")]
    public void StaticTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public static void Method()
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
      expectedTS.AppendLine("  public static Method(): void {");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}
