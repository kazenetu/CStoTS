using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("ローカルメソッドのテスト", nameof(LocalFunctionTest))]
  public class LocalFunctionTest : TestBase
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
          void VoidMethod()
          {
          }
          int IntMethod()
          {
            return 0;
          }
          string StringMethod()
          {
            return ""ABC"";
          }

          VoidMethod();
          IntMethod();
          StringMethod();
          var intValue = IntMethod();
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
      expectedTS.AppendLine("    let VoidMethod = (): void => {");
      expectedTS.AppendLine("    };");
      expectedTS.AppendLine("    let IntMethod = (): number => {");
      expectedTS.AppendLine("      return 0;");
      expectedTS.AppendLine("    };");
      expectedTS.AppendLine("    let StringMethod = (): string => {");
      expectedTS.AppendLine("      return \"ABC\";");
      expectedTS.AppendLine("    };");
      expectedTS.AppendLine("    VoidMethod();");
      expectedTS.AppendLine("    IntMethod();");
      expectedTS.AppendLine("    StringMethod();");
      expectedTS.AppendLine("    let intValue: number = IntMethod();");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "DeclarationClassTest")]
    public void DeclarationClassTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public static void Method()
        {
          void VoidMethod()
          {
          }
          VoidMethod();
        }

        public class Inner {
          public static void Method()
          {
            void VoidMethod()
            {
            }
            VoidMethod();
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
      expectedTS.AppendLine("    let VoidMethod = (): void => {");
      expectedTS.AppendLine("    };");
      expectedTS.AppendLine("    VoidMethod();");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      expectedTS.AppendLine("export namespace Test {");
      expectedTS.AppendLine("  export class Inner {");
      expectedTS.AppendLine("    public Method(): void {");
      expectedTS.AppendLine("      let VoidMethod = (): void => {");
      expectedTS.AppendLine("      };");
      expectedTS.AppendLine("      VoidMethod();");
      expectedTS.AppendLine("    }");
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
          CompassDirection EnumMethod()
          {
            return CompassDirection.East;
          }
          EnumMethod();
          var enumValue = EnumMethod();
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
      expectedTS.AppendLine("    let EnumMethod = (): CompassDirection => {");
      expectedTS.AppendLine("      return CompassDirection.East;");
      expectedTS.AppendLine("    };");
      expectedTS.AppendLine("    EnumMethod();");
      expectedTS.AppendLine("    let enumValue: CompassDirection = EnumMethod();");
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
