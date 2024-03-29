using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("クラスのテスト", nameof(ClassTest))]
  public class ClassTest : TestBase
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
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// 内部クラスのテスト
    /// </summary>
    [Fact(DisplayName = "InnerClassTest")]
    public void InnerClassTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public class Inner {
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
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// サブクラスのテスト
    /// </summary>
    [Fact(DisplayName = "SubClassTest")]
    public void SubClassTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
      }");
      CreateFileData("sub.cs", string.Empty,
      @"public class Sub:Test
      {
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
      @"public class Test<T,U,V>
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test<T, U, V> {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// ジェネリクスのサブクラスのテスト
    /// </summary>
    [Fact(DisplayName = "GenericsSubTest")]
    public void GenericsSubTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test<T>
      {
      }");
      CreateFileData("sub.cs", string.Empty,
      @"public class Sub<T>:Test<T>
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test<T> } from './test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub<T> extends Test<T> {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// ジェネリクススーパークラスのサブクラスのテスト
    /// </summary>
    [Fact(DisplayName = "GenericsSubFixedTypeTest")]
    public void GenericsSubFixedTypeTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test<T,U,V>
      {
      }");
      CreateFileData("sub.cs", string.Empty,
      @"
      public class Sub :Test<string, int, decimal>
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Test<T, U, V> } from './test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub extends Test<string, number, number> {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// コメントのテスト
    /// </summary>
    [Fact(DisplayName = "CommentTest")]
    public void CommentTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"
      /// <summary>
      /// クラスコメント
      /// </summary>
      /// <typeparam name=""T"">A</typeparam>
      /// <typeparam name=""U"">B</typeparam>
      /// <typeparam name=""V"">C</typeparam>
      public class Test<T,U,V>
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("/**");
      expectedTS.AppendLine(" * クラスコメント");
      expectedTS.AppendLine(" * @template T A");
      expectedTS.AppendLine(" * @template U B");
      expectedTS.AppendLine(" * @template V C");
      expectedTS.AppendLine(" */");
      expectedTS.AppendLine("export class Test<T, U, V> {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// インターフェース継承のテスト
    /// </summary>
    [Fact(DisplayName = "InterfaceTest")]
    public void InterfaceTest()
    {
      // C#ソース作成
      CreateFileData("interface.cs", string.Empty,
      @"public interface Inf
      {
      }");
      CreateFileData("test.cs", string.Empty,
      @"public class Test<T, U, V>
      {
      }");
      CreateFileData("sub.cs", string.Empty,
      @"
      public class Sub :Test<string, int, decimal>,Inf
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Inf } from './interface';");
      expectedTS.AppendLine("import { Test<T, U, V> } from './test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub extends Test<string, number, number> implements Inf {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// インターフェース多重継承のテスト
    /// </summary>
    [Fact(DisplayName = "MultiInterfaceTest")]
    public void MultiInterfaceTest()
    {
      // C#ソース作成
      CreateFileData("interface.cs", string.Empty,
      @"public interface Inf
      {
      }");
      CreateFileData("interface2.cs", string.Empty,
      @"public interface Inf2<T, V>
      {
      }");
      CreateFileData("test.cs", string.Empty,
      @"public class Test<T, U, V>
      {
      }");
      CreateFileData("sub.cs", string.Empty,
      @"
      public class Sub :Test<string, int, decimal>,Inf,Inf2<int,decimal>
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("sub.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { Inf } from './interface';");
      expectedTS.AppendLine("import { Inf2<T, V> } from './interface2';");
      expectedTS.AppendLine("import { Test<T, U, V> } from './test';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Sub extends Test<string, number, number> implements Inf, Inf2<number, number> {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// staticクラスのテスト
    /// </summary>
    [Fact(DisplayName = "StaticTest")]
    public void StaticTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public static class Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// 複数クラス定義のテスト
    /// </summary>
    [Fact(DisplayName = "MultiClassTest")]
    public void MultiClassTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
      }
      public class MyClass
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("}");
      expectedTS.AppendLine("export class MyClass {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}
