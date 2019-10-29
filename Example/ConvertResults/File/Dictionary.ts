/** Dictionary クラス */
export class Dictionary<K, V> implements Iterable<[K, V]>
{
  /**
   * Mapインスタンス
   */
  private instance: Map<K, V>;

  /**
   * 要素数
   */
  public get Count(): number {
    return this.instance.size;
  }

  /**
   * 全てのキー取得
   */
  public get Keys(): IterableIterator<K> {
    return this.instance.keys();
  }

  /**
   * 全ての値取得
   */
  public get Values(): IterableIterator<V> {
    return this.instance.values();
  }

  /**
   * コンストラクタ
   */
  public constructor() {
    this.instance = new Map<K, V>();
  }

  /**
   * 値取得
   * @param key 取得キー
   */
  public get(key: K): V {
    return this.instance.get(key) as V;
  }

  /**
   * 要素の追加
   * @param key キー
   * @param value 値
   */
  public Add(key: K, value: V): void {
    this.instance.set(key, value);
  }

  /**
   * クリア
   */
  public Clear(): void {
    this.instance.clear();
  }

  /**
   * キーの存在確認
   * @param key 確認対象のキー
   */
  public ContainsKey(key: K): boolean {
    return this.instance.has(key);
  }

  /**
   * 値の存在確認
   * @param value 確認対象の値 
   */
  public ContainsValue(value: V): boolean {
    let result = false;
    this.instance.forEach((val) => {
      if (val === value) {
        result = true;
      }
    });
    return result;
  }

  /**
   * 要素の削除
   * @param key 削除対象のキー
   * @param value 削除対象の値(省略時null)
   */
  public Remove(key: K, value: V | null = null): boolean {

    // キーがない場合はfalseを返す
    if (!this.instance.has(key)) {
      return false;
    }

    // 値が設定されていて不一致の場合はfalseを返す
    if (value !== null && this.instance.get(key) !== value) {
      return false;
    }

    // 削除結果を返す
    return this.instance.delete(key);
  }
  toJSON() {
    let result = new Array();
    this.instance.forEach((value, key) => {
      result.push([key, value]);
    });

    return result;
  }
  fromJSON(value: any) {
    this.instance = new Map(value);
  }

  /**
   * イテレーター
   */
  public [Symbol.iterator](): Iterator<[K, V]> {
    return this.instance[Symbol.iterator]()
  }
}
