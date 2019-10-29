/**
 * メソッドテストクラス
 */
export class TestMethod {
  /**
   * 定数フィールド
   */
  public ConstField: string = "123";
  private _prop_: string = "";
  /**
   * プロパティ例
   */
  public set prop(value: string) {
    this._prop_ = value;
  }
  /**
   * プロパティ例
   */
  public get prop(): string {
    return this._prop_;
  }
  private _propInt_: number = 50;
  public set propInt(value: number) {
    this._propInt_ = value;
  }
  public get propInt(): number {
    return this._propInt_;
  }
}
export namespace TestMethod {
  export class InnerClass {
    public static StaticField: string = "789";
  }
}
