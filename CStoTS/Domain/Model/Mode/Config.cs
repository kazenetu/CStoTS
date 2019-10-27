namespace CStoTS.Domain.Model.Mode
{
  /// <summary>
  /// 設定情報
  /// </summary>
  public class Config
  {
    /// <summary>
    /// 出力モード
    /// </summary>
    public OutputMode Mode { get; private set; }

    /// <summary>
    /// 入力：C#のルートパス
    /// </summary>
    public RootPath InputCSRoot { get; private set; }

    /// <summary>
    /// 入力：C#のファイル指定
    /// </summary>
    public string InputFile { get; private set; }

    /// <summary>
    /// 出力：TypeScriptのルートパス
    /// </summary>
    public RootPath OutputTSRoot { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="mode">出力モード</param>
    /// <param name="inputCSRoot">入力：C#のルートパス</param>
    /// <param name="inputFile">出力：TypeScriptのルートパス</param>
    /// <param name="outputTSRoot">出力：TypeScriptのルートパス</param>
    private Config(OutputMode mode, RootPath inputCSRoot, RootPath outputTSRoot, string inputFile)
    {
      Mode = mode;
      InputCSRoot = inputCSRoot;
      InputFile = inputFile;
      OutputTSRoot = outputTSRoot;
    }

    /// <summary>
    /// インスタンス取得
    /// </summary>
    /// <param name="inputCSRoot">入力：C#のルートパス</param>
    /// <param name="outputTSRoot">出力：TypeScriptのルートパス</param>
    /// <param name="inputFile">出力：TypeScriptのルートパス</param>
    /// <returns></returns>
    public static Config Create(string inputCSRoot, string outputTSRoot, string inputFile = null)
    {
      return new Config(OutputMode.Create(OutputMode.Mode.All), RootPath.Create(inputCSRoot), RootPath.Create(outputTSRoot), inputFile);
    }

    /// <summary>
    /// インスタンス取得
    /// </summary>
    /// <param name="mode">出力モード</param>
    /// <param name="inputCSRoot">入力：C#のルートパス</param>
    /// <param name="outputTSRoot">出力：TypeScriptのルートパス</param>
    /// <param name="inputFile">出力：TypeScriptのルートパス</param>
    /// <returns></returns>
    public static Config Create(OutputMode.Mode mode, string inputCSRoot, string outputTSRoot, string inputFile = null)
    {
      return new Config(OutputMode.Create(mode), RootPath.Create(inputCSRoot), RootPath.Create(outputTSRoot), inputFile);
    }
  }
}
