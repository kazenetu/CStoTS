using System;
using System.Collections.Generic;

namespace DataTransferObjects
{
  public class OtherClass
  {
    public class InnerClass
    {
      public static string StaticField = "789";

      public static string StaticMethod()
      {
        return "aaa";
      }

      public static string StaticMethodArg(string name)
      {
        return "hellow!" + name;
      }
    }
    public static string StaticField = "456";
    public static string StaticMethod()
    {
      return "bbb";
    }
  }
}