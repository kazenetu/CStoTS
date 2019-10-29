/**
 * Initialize リクエスト
 */
export class InitializeRequest extends RequestBase {
  private _OrderNo_: number;
  /**
   * 注文番号
   */
  public set OrderNo(value: number) {
    this._OrderNo_ = value;
  }
  /**
   * 注文番号
   */
  public get OrderNo(): number {
    return this._OrderNo_;
  }
}
