/**
 * FindUserName リクエスト
 */
export class FindUserNameRequest extends RequestBase {
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
}
