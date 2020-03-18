using System.Collections.Generic;

namespace ReadLine {
  public interface IReadLine {
    IAutoCompleteHandler AutoCompletionHandler { get; set; }
    void AddHistory(params string[] text);
    List<string> GetHistory();
    void ClearHistory();
    string Read(string prompt = "", string @default = "");
    string ReadPassword(string prompt = "");
  }
}
