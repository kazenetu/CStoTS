using System;
using System.Collections.Generic;

namespace DataTransferObjects
{
  /// <summary>
  /// メソッドテストクラス
  /// </summary>
  public class TestMethod
  {
    public class InnerClass
    {
      public static string StaticField = "789";

      public static string StaticMethod()
      {
        return "bbb";
      }

      public static string StaticMethodArg(string name)
      {
        return "hey!" + name;
      }
    }
    public static string StaticMethod()
    {
      return "ccc";
    }

    /// <summary>
    /// 定数フィールド
    /// </summary>
    public const string ConstField = "123";

    /// <summary>
    /// プロパティ例
    /// </summary>
    public string prop { set; get; } = "";

    public int propInt { set; get; } = 50;

    /// <summary>
    /// メソッド例
    /// </summary>
    public void Method()
    {
      // this.prop = TestMethod.StaticMethod()
      prop = StaticMethod();
      // this.prop = TestLogic.StaticMethod()
      prop = TestMethod.StaticMethod();

      // this.prop = TestMethod.InnerClass.StaticMethod()
      prop = InnerClass.StaticMethod();
      // this.prop = TestMethod.InnerClass.StaticMethod()
      prop = TestMethod.InnerClass.StaticMethod();
      // this.prop = TestMethod.InnerClass.StaticMethodArg("test")
      prop = TestMethod.InnerClass.StaticMethodArg("test");
      // this.prop = TestMethod.InnerClass.StaticMethodArg(propInt.toString())
      prop = TestMethod.InnerClass.StaticMethodArg(propInt.ToString());

      // this.prop = OtherClass.StaticMethod()
      prop = OtherClass.StaticMethod();

      // this.prop = OtherClass.InnerClass.StaticMethod()
      prop = OtherClass.InnerClass.StaticMethod();
      // this.prop = OtherClassc.InnerClass.StaticMethodArg("test")
      prop = OtherClass.InnerClass.StaticMethodArg("test");
      // this.prop = OtherClass.InnerClass.StaticMethodArg(propInt.toString())
      prop = OtherClass.InnerClass.StaticMethodArg(propInt.ToString());
    }

  }
}
