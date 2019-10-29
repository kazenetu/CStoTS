import { Dictionary } from '../Dictionary';
import { PropertyInfo } from '../PropertyInfo';
import { Type } from '../Type';

/**
 * ユーザーマスタ―
 */
export class MtUser extends TableBase {
  /**
   * DBカラム名とプロパティのコレクション取得
   * @returns DBカラム名とプロパティのコレクション
   */
  public GetDBComlunProperyColection(): Dictionary<string, PropertyInfo> {
    let result: Dictionary<string, PropertyInfo> = new Dictionary<string, PropertyInfo>();
    let classType: Type = this.GetType();
    result.Add("USER_ID", classType.GetProperty("UserId"));
    result.Add("USER_NAME", classType.GetProperty("UserName"));
    result.Add("PASSWORD", classType.GetProperty("Password"));
    result.Add("DEL_FLAG", classType.GetProperty("DelFlag"));
    result.Add("ENTRY_USER", classType.GetProperty("EntryUser"));
    result.Add("ENTRY_DATE", classType.GetProperty("EntryDate"));
    result.Add("MOD_USER", classType.GetProperty("ModUser"));
    result.Add("MOD_DATE", classType.GetProperty("ModDate"));
    result.Add("MOD_VERSION", classType.GetProperty("ModVersion"));
    return result;
  }
  private _UserId_: string;
  /**
   * ユーザーID
   */
  public set UserId(value: string) {
    this._UserId_ = value;
  }
  /**
   * ユーザーID
   */
  public get UserId(): string {
    return this._UserId_;
  }
  private _UserName_: string;
  /**
   * ユーザー名
   */
  public set UserName(value: string) {
    this._UserName_ = value;
  }
  /**
   * ユーザー名
   */
  public get UserName(): string {
    return this._UserName_;
  }
  private _Password_: string;
  /**
   * パスワード
   */
  public set Password(value: string) {
    this._Password_ = value;
  }
  /**
   * パスワード
   */
  public get Password(): string {
    return this._Password_;
  }
  private _DelFlag_: string;
  /**
   * 削除フラグ
   */
  public set DelFlag(value: string) {
    this._DelFlag_ = value;
  }
  /**
   * 削除フラグ
   */
  public get DelFlag(): string {
    return this._DelFlag_;
  }
  private _EntryUser_: string;
  /**
   * 登録ユーザー
   */
  public set EntryUser(value: string) {
    this._EntryUser_ = value;
  }
  /**
   * 登録ユーザー
   */
  public get EntryUser(): string {
    return this._EntryUser_;
  }
  private _EntryDate_: Date?;
  /**
   * 登録日時
   */
  public set EntryDate(value: Date?) {
    this._EntryDate_ = value;
  }
  /**
   * 登録日時
   */
  public get EntryDate(): Date? {
    return this._EntryDate_;
  }
  private _ModUser_: string;
  /**
   * 更新ユーザー
   */
  public set ModUser(value: string) {
    this._ModUser_ = value;
  }
  /**
   * 更新ユーザー
   */
  public get ModUser(): string {
    return this._ModUser_;
  }
  private _ModDate_: Date?;
  /**
   * 更新日時
   */
  public set ModDate(value: Date?) {
    this._ModDate_ = value;
  }
  /**
   * 更新日時
   */
  public get ModDate(): Date? {
    return this._ModDate_;
  }
  private _ModVersion_: number?;
  /**
   * 更新バージョン
   */
  public set ModVersion(value: number?) {
    this._ModVersion_ = value;
  }
  /**
   * 更新バージョン
   */
  public get ModVersion(): number? {
    return this._ModVersion_;
  }
}
