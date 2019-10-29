using System;
using System.Collections.Generic;

namespace DataTransferObjects
{
  /// <summary>
  /// テストクラス
  /// </summary>
  public class TestLogic
  {
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
      int a(int arg){
        var b = arg;
        b += arg;
        return b;
      }

      int test = a(10);
      if (prop.Length > 0)
      {
        test = this.Method2(prop);
      }

      // while構文(インクリメント)
      var index = 0;
      while (index < 9)
      {
        propInt = index;

        // 条件用変数をインクリメント
        index++;
      }

      // while構文(デクリメント)
      index = 9;
      while (index > 0)
      {
        test = index;

        // 条件用変数をデクリメント
        index--;
      }

      var dc = new Dictionary<string,TestLogic>();
      dc.Add("aa",this);
      var value = dc["aa"];
      foreach(var key in dc.Keys){
        test = dc[key].propInt;
        var strTest = dc[key].ToString();
      }
      prop = dc.Keys.ToString();
      dc.Clear();

      var lst = new List<string>();
      lst.Add("aaa");
      foreach(var item in lst){
        this.prop = item;
      }
      lst.Remove("aaa");
      lst.Clear();

      prop = string.Empty;
      prop = ConstField;
    }

    /// <summary>
    /// メソッド例2 A
    /// </summary>
    /// <param name="src">文字列</param>
    /// <returns>戻り値</returns>
    public int Method2(string src)
    {
      return int.Parse(src);
    }

    /// <summary>
    /// メソッド例2 B
    /// </summary>
    /// <param name="src">数値</param>
    /// <returns>戻り値</returns>
    public int Method2(int src)
    {
      return src * 2;
    }

    /// <summary>
    /// コンストラクタ
    /// /// </summary>
    public TestLogic()
    {
      // ローカル変数宣言確認(型推論)
      var local = 123;

      // ローカル変数の値分岐
      if (local >= 10)
      {
        prop = local.ToString();
        local = 1;
      }
      else
      {
        // ローカル変数宣言確認(型指定)
        int test = 0;
      }

      // プロパティの値分岐
      if(prop == "123"){
        var localString = "";
        localString = prop;
      }

      // ローカル変数でのswitch
      switch (local)
      {
        case 1:
          prop = "1";
          break;
        case 2:
          prop = "2";
          break;
        case 3:
        case 4:
          prop = "34";
          break;
        default:
          prop = "333";
          break;
      }

      // for分確認
      for (var i = 0; i < 10; i++)
      {
        local = i;
        prop = local.ToString();
      }

      // for分確認(プロパティ)
      for (var i = 0; i < propInt; i++)
      {
        local = i;
        prop = local.ToString();
      }

      // 計算代入式1
      local += local *3;

      // 計算代入式2
      local -= local /propInt;
      local = local.ToString().Length;
    }
 
    /// <summary>
    /// 複数コンストラクタ1
    /// </summary>
    /// <param name="paramValue">パラメータ</param>
    public TestLogic(int paramValue)
    {
      prop = paramValue.ToString();
    }

    /// <summary>
    /// 複数コンストラクタ2
    /// </summary>
    /// <param name="param">パラメータ</param>
    public TestLogic(string param)
    {
      prop = "コンストラクタ";
    }

    /// <summary>
    /// 複数コンストラクタ3
    /// </summary>
    /// <param name="param">パラメータ1</param>
    /// <param name="boolValue">パラメータ2</param>
    public TestLogic(string param,bool boolValue)
    {
      prop = "コンストラクタ";
    }

    /// <summary>
    /// 複数コンストラクタ4
    /// </summary>
    /// <param name="param">パラメータ1</param>
    /// <param name="dateValue">パラメータ2</param>
    public TestLogic(string param,DateTime dateValue)
    {
      param = "コンストラクタ";
    }

  }
}
