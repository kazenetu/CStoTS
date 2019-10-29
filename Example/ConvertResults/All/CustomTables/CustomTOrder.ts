import { Dictionary } from '../Dictionary';
import { PropertyInfo } from '../PropertyInfo';
import { TOrder } from '../Tables/TOrder';
import { Type } from '../Type';

/**
 * 注文一覧用カスタムテーブルDTO
 */
export class CustomTOrder extends TOrder {
  /**
   * DBカラム名とプロパティのコレクション取得
   * @returns DBカラム名とプロパティのコレクション
   */
  public GetDBComlunProperyColection(): Dictionary<string, PropertyInfo> {
    let result: Dictionary<string, PropertyInfo> = this.GetDBComlunProperyColection();
    let classType: Type = this.GetType();
    result.Add("ORDER_USER_NAME", classType.GetProperty("OrderUserName"));
    return result;
  }
  private _OrderUserName_: string;
  /**
   * 注文者名
   */
  public set OrderUserName(value: string) {
    this._OrderUserName_ = value;
  }
  /**
   * 注文者名
   */
  public get OrderUserName(): string {
    return this._OrderUserName_;
  }
}
