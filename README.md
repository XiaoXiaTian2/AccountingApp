# AccountingApp

## 待改進

1. **`DebounceClick` 的 static timer 只會執行第一次的 callback**：`Utility/FormExtension.cs` 的 `timer` 是 static 欄位，只有在 `timer == null` 時才用當下的 `callback` 建立 `System.Threading.Timer`，之後的呼叫只做 `timer.Change(due, -1)`。也就是說，之後傳入不同的 callback（或不同的 Form）都不會生效，到期時仍執行第一次綁定的那個；兩個多載還共用同一個 timer。目前只有 `記帳本.button1_Click` 一處呼叫，而且 lambda 每次都讀同一個 Form 的控制項，所以還看不出症狀，但只要第二個地方使用就會出錯。修正方向：以 Form 或按鈕為 key 各自保存 timer，或每次都更新要執行的 callback。
2. **刪除後重繪會因圖片路徑為 null 而丟例外**：刪除流程用 `Img1` 當識別 key（`RecordRepository.DeleteAccount` 以 `rec.Img1 != recordDAO.Img1` 過濾，`EditRecords` 也用 `Img1` 找目標）。因為新增時圖片檔名用 `Guid.NewGuid()`，目前 key 不會重複，但識別依賴檔案路徑本身就很脆弱。更直接的問題在 `Views/記帳本.cs`：`OnDeleteAccount` 把 DTO 轉回 `RecordModel` 時漏掉了 `Img1`、`Img2`，接著 `dataGridView1_CellDoubleClick` 呼叫 `ReRender()`，對每一列執行 `ImageSet(rows[i].Cells["Img1"].Value?.ToString())`，傳入 null 給 `File.ReadAllBytes` 會丟出例外。也就是刪除後只要還有剩下的資料，重繪就會失敗。修正方向：替記錄加上獨立的 Id 欄位，並在 `OnDeleteAccount` 補上圖片路徑。
