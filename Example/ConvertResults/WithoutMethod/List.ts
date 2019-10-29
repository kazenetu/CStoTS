/** List クラス */
export class List<V> implements Iterable<V>
{
  /**
   * Arrayインスタンス
   */
  private instance: Array<V>;

  /**
   * 要素数
   */
  public get Count(): number {
    return this.instance.length;
  }

  /**
   * 現在の要素数
   */
  public get Capacity(): number {
    return this.instance.length;
  }

  /**
   * 現在の要素数
   */
  public set Capacity(length: number) {
    this.instance.length = length;
  }

  /**
   * コンストラクタ
   */
  public constructor() {
    this.instance = new Array<V>();
  }

  /**
   * 値取得
   * @param index 取得対象の要素インデックス
   */
  public get(index: number): V {
    return this.instance[index];
  }

  /**
   * 要素の追加
   * @param value 値
   */
  public Add(value: V): void {
    this.instance.push(value);
  }

  public AddRange(values: Array<V>): void {
    this.instance = this.instance.concat(values);
  }

  /**
   * クリア
   */
  public Clear(): void {
    this.instance.length = 0;
  }

  /**
   * 値の存在確認
   * @param value 確認対象の値 
   */
  public Contains(value: V): boolean {
    if (this.instance.indexOf(value) < 0) {
      return false;
    }
    return true;
  }

  /**
   * 要素の削除
   * @param value 削除対象の値
   */
  public Remove(value: V): boolean {

    // 値がない場合はfalseを返す
    if (!this.Contains(value)) {
      return false;
    }

    // 削除処理
    const removeIndex = this.instance.indexOf(value);
    this.instance = this.instance.filter((val, index) => index !== removeIndex);

    // trueを返す
    return true;
  }
  toJSON() {
    return this.instance;
  }
  fromJSON(value: any) {
    this.instance = new Array(value);
  }

  /**
   * イテレーター
   */
  public [Symbol.iterator](): Iterator<V> {
    return this.instance[Symbol.iterator]()
  }
}