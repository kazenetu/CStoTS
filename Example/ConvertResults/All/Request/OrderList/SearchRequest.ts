/**
 * Search リクエスト
 */
export class SearchRequest extends RequestBase {
  private _SearchUserID_: string;
  /**
   * 検索条件:ユーザーID
   */
  public set SearchUserID(value: string) {
    this._SearchUserID_ = value;
  }
  /**
   * 検索条件:ユーザーID
   */
  public get SearchUserID(): string {
    return this._SearchUserID_;
  }
}
