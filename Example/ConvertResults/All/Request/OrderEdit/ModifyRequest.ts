/**
 * Modify リクエスト
 */
export class ModifyRequest extends RequestBase {
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
  private _OrderUserID_: string;
  /**
   * 注文者ID
   */
  public set OrderUserID(value: string) {
    this._OrderUserID_ = value;
  }
  /**
   * 注文者ID
   */
  public get OrderUserID(): string {
    return this._OrderUserID_;
  }
  private _ModVersion_: number;
  /**
   * 更新バージョン
   */
  public set ModVersion(value: number) {
    this._ModVersion_ = value;
  }
  /**
   * 更新バージョン
   */
  public get ModVersion(): number {
    return this._ModVersion_;
  }
}
