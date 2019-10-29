import { Dictionary } from '../Dictionary';
import { PropertyInfo } from '../PropertyInfo';

/**
 * ダミーテーブルDTO
 */
export class DummyTable extends TableBase {
  /**
   * DBカラム名とプロパティのコレクション取得
   * @returns DBカラム名とプロパティのコレクション
   */
  public GetDBComlunProperyColection(): Dictionary<string, PropertyInfo> {
    let result: Dictionary<string, PropertyInfo> = new Dictionary<string, PropertyInfo>();
    return result;
  }
}
