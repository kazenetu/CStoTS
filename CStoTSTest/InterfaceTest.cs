﻿using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("インターフェースのテスト", nameof(InterfaceTest))]
  public class InterfaceTest : TestBase
  {
    /// <summary>
    /// 標準テスト
    /// </summary>
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public interface Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export interface Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// ジェネリクス使用のテスト
    /// </summary>
    [Fact(DisplayName = "GenericsTest")]
    public void GenericsTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public interface Test<T>
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export interface Test<T> {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// ジェネリクスインターフェースのテスト
    /// </summary>
    [Fact(DisplayName = "GenericsSubFixedTypeTest")]
    public void GenericsSubFixedTypeTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public interface Test<T,U,V>
      {
      }");
      CreateFileData("sub.cs", string.Empty,
      @"
      public interface Sub :Test<string, int, decimal>
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
      expectedTS.AppendLine("export interface Sub implements Test<string, number, number> {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// Protectedスコープのテスト
    /// </summary>
    [Fact(DisplayName = "ProtectedScopeTest")]
    public void ProtectedScopeTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"protected interface Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export interface Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// Internalスコープのテスト
    /// </summary>
    [Fact(DisplayName = "InternalScopeTest")]
    public void InternalScopeTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"internal interface Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export interface Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// Privateスコープのテスト
    /// </summary>
    [Fact(DisplayName = "PrivateScopeTest")]
    public void PrivateScopeTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"private interface Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("interface Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// スコープなしのテスト
    /// </summary>
    [Fact(DisplayName = "NotExitsScopeTest")]
    public void NotExitsScopeTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"interface Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("interface Test {");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// メンバー定義のテスト
    /// </summary>
    [Fact(DisplayName = "MemberTest")]
    public void MemberTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public interface Test
      {
        int field;
        void Method();
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export interface Test {");
      expectedTS.AppendLine("  field: number;");
      expectedTS.AppendLine("  Method(): void;");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// メンバー定義(パラメータありメソッド)のテスト
    /// </summary>
    [Fact(DisplayName = "MultiMemberTest")]
    public void MultiMemberTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public interface Test
      {
        void Method();
        void Method(int param);
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export interface Test {");
      expectedTS.AppendLine("  Method(): void;");
      expectedTS.AppendLine("  Method(param: number): void;");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    /// <summary>
    /// ジェネリクスメンバーのテスト
    /// </summary>
    [Fact(DisplayName = "GenericsMemberTest")]
    public void GenericsMemberTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public interface Test
      {
        void Method<T>(T param);
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export interface Test {");
      expectedTS.AppendLine("  Method<T>(param: T): void;");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}
