/**
 * Simple JSONConverter for TypeScript
 * Copyright 2019, kazenetu
 * Released under the MIT License.
 */
export class JSONConverter<T>
{
  public serialize(target: T): string {
    return JSON.stringify(target);
  }
  public deserialize(jsonString: string, newInstance: T): T {
    return this.deserializeMapping(newInstance, JSON.parse(jsonString));
  }
  private deserializeMapping(newInstance: any, jsonObject: any): T {
    for (let itemName in jsonObject) {
      let itemObject = jsonObject[itemName];

      // プロパティ用フィールドの存在確認と対象名の再設定
      let newInstanceItemName = itemName;
      if (newInstance.hasOwnProperty("_" + itemName + "_")) {
        newInstanceItemName = "_" + itemName + "_";
      }

      if (typeof itemObject === 'object') {
        this.deserializeMapping(newInstance[newInstanceItemName], itemObject);
      } else {
        newInstance[newInstanceItemName] = jsonObject[itemName];
      }
    }
    return newInstance;
  }
}