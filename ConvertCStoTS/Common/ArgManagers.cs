using System.Collections.Generic;

namespace ConvertCStoTS.Common
{
  /// <summary>
  /// パラメータ管理クラス
  /// </summary>
  public class ArgManagers
  {
    /// <summary>
    /// オプションパラメータ(名前あり)のコレクション
    /// </summary>
    private Dictionary<string, string> OptionPramArgs = new Dictionary<string, string>();

    /// <summary>
    /// 必須パラメーター(名前なし)のコレクション
    /// </summary>
    private List<string> RequiredPramArgs = new List<string>();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="args">パラメータ</param>
    /// <remarks>パラメータのリストを作成する</remarks>
    public ArgManagers(string[] args)
    {
      var paramName = string.Empty;
      foreach (var arg in args)
      {
        switch (arg[0])
        {
          case '-':
          case '/':
            if (!string.IsNullOrEmpty(paramName))
            {
              // パラメータ名だけの場合はパラメータ名だけ追加
              OptionPramArgs.Add(paramName, null);
            }

            // パラメータ名を設定
            paramName = arg.Substring(0, 1).Replace("/", "-", System.StringComparison.CurrentCulture) +
                        arg.Substring(1);
            break;
          default:

            // パラメータの追加
            if (string.IsNullOrEmpty(paramName))
            {
              // パラメータ名なし
              RequiredPramArgs.Add(arg);
            }
            else
            {
              // パラメータ名あり
              OptionPramArgs.Add(paramName, arg);
            }

            // パラメータ名をクリア
            paramName = string.Empty;
            break;
        }
      }

      // パラメータ名だけの場合はパラメータ名だけ追加
      if (!string.IsNullOrEmpty(paramName))
      {
        OptionPramArgs.Add(paramName, null);
      }
    }

    /// <summary>
    /// 必須パラメータの取得
    /// </summary>
    /// <param name="index">取得パラメータのインデックス</param>
    /// <returns>対象パラメータの値(インデックスが存在しない場合はnull)</returns>
    public string GetRequiredArg(int index)
    {
      if (RequiredPramArgs.Count <= index)
      {
        return null;
      }

      return RequiredPramArgs[index];
    }

    /// <summary>
    /// 必須パラメータ数の取得
    /// </summary>
    /// <returns>必須パラメータ数</returns>
    public int GetRequiredArgCount()
    {
      return RequiredPramArgs.Count;
    }

    /// <summary>
    /// オプションパラメータの取得
    /// </summary>
    /// <param name="paramName">パラメータ名</param>
    /// <returns>対象パラメータの値(パラメータ名が存在しない場合はnull)</returns>
    public string GetOptionArg(string paramName)
    {
      if (!OptionPramArgs.ContainsKey(paramName))
      {
        return null;
      }

      return OptionPramArgs[paramName];
    }

    /// <summary>
    /// オプションパラメータの取得
    /// </summary>
    /// <param name="paramName">パラメータ名リスト</param>
    /// <returns>対象パラメータの値(パラメータ名が存在しない場合はnull)</returns>
    public string GetOptionArg(List<string> paramNames)
    {
      string result = null;
      foreach (var pramName in paramNames)
      {
        result = GetOptionArg(pramName);
        if (result != null)
        {
          break;
        }
      }
      return result;
    }

    /// <summary>
    /// オプションパラメータ名の存在確認
    /// </summary>
    /// <param name="paramName">パラメータ名</param>
    /// <returns>確認結果</returns>
    public bool ExistsOptionArg(string paramName)
    {
      return OptionPramArgs.ContainsKey(paramName);
    }

    /// <summary>
    /// オプションパラメータ名の存在確認
    /// </summary>
    /// <param name="paramName">パラメータ名リスト</param>
    /// <returns>確認結果</returns>
    public bool ExistsOptionArg(List<string> paramNames)
    {
      var result = false;
      foreach (var pramName in paramNames)
      {
        result = ExistsOptionArg(pramName);
        if (result)
        {
          break;
        }
      }
      return result;
    }
  }
}
