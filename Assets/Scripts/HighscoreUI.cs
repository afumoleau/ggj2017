using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HighscoreUI : MonoBehaviour {

	[SerializeField] GameObject[] entries;
	[SerializeField] Text[] names;
	[SerializeField] Text[] scores;

	public void GetHighscores() {
		foreach(var entry in entries) {
			entry.SetActive(false);
		}
		FirebaseDatabase.DefaultInstance.GetReference("scores").OrderByChild("score")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					Debug.LogError("Get score request failed");
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					var i = 0;
					foreach(var scoreEntry in snapshot.Children.Reverse()) {
						if(i >= 7) {
							break;
						}
						ScoreManager.LeaderboardEntry entry = JsonUtility.FromJson<ScoreManager.LeaderboardEntry>(scoreEntry.GetRawJsonValue());
						entries[i].SetActive(true);
						names[i].text = entry.name;
						scores[i].text = "" + entry.score;
						i++;
					}
				}
			});
	}
}
