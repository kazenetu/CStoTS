import { Dictionary } from '../Dictionary';
import { PropertyInfo } from '../PropertyInfo';
import { Type } from '../Type';

/**
 * 注文
 */
export class TOrder extends TableBase {
  /**
   * DBカラム名とプロパティのコレクション取得
   * @returns DBカラム名とプロパティのコレクション
   */
  public GetDBComlunProperyColection(): Dictionary<string, PropertyInfo> {
    let result: Dictionary<string, PropertyInfo> = new Dictionary<string, PropertyInfo>();
    let classType: Type = this.GetType();
    result.Add("ORDER_NO", classType.GetProperty("OrderNo"));
    result.Add("ORDER_USER_ID", classType.GetProperty("OrderUserId"));
    result.Add("MOD_VERSION", classType.GetProperty("ModVersion"));
    return result;
  }
  private _OrderNo_: number?;
  /**
   * 注文No
   */
  public set OrderNo(value: number?) {
    this._OrderNo_ = value;
  }
  /**
   * 注文No
   */
  public get OrderNo(): number? {
    return this._OrderNo_;
  }
  private _OrderUserId_: string;
  /**
   * 注文者ID
   */
  public set OrderUserId(value: string) {
    this._OrderUserId_ = value;
  }
  /**
   * 注文者ID
   */
  public get OrderUserId(): string {
    return this._OrderUserId_;
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
