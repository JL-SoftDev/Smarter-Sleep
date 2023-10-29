import 'package:flutter/material.dart';

class SleepReviewScreen extends StatelessWidget {
  final List<Map<String, dynamic>> sleepData = [
    {
      "id": 1,
      "userId": "2442bede-d781-49d7-99fa-9025c5b35b15",
      "createdAt": "2023-10-20T21:24:59.395986",
      "smarterSleepScore": 57,
      "survey": {
        "id": 1,
        "createdAt": "2023-10-20T21:24:59.396077",
        "sleepQuality": 3,
        "wakePreference": 1,
        "temperaturePreference": 0,
        "lightsDisturbance": true,
        "sleepEarlier": false,
        "sleepDuration": 300,
        "surveyDate": "2023-10-20"
      },
      "wearableLog": {
        "id": 1,
        "sleepStart": "2023-10-20T21:24:59.396289",
        "sleepEnd": "2023-10-21T05:24:59.396312",
        "hypnogram":
            "444432221111122333332222222222223111133333322221112233333333332232222334",
        "sleepScore": 55,
        "sleepDate": "2023-10-20"
      }
    },
    {
      "id": 2,
      "userId": "2442bede-d781-49d7-99fa-9025c5b35b15",
      "createdAt": "2023-10-21T21:24:59.395986",
      "smarterSleepScore": 59,
      "survey": {
        "id": 2,
        "createdAt": "2023-10-21T21:24:59.396077",
        "sleepQuality": 4,
        "wakePreference": 2,
        "temperaturePreference": 1,
        "lightsDisturbance": false,
        "sleepEarlier": false,
        "sleepDuration": 250,
        "surveyDate": "2023-10-21"
      },
      "wearableLog": {
        "id": 2,
        "sleepStart": "2023-10-21T21:24:59.396289",
        "sleepEnd": "2023-10-22T05:24:59.396312",
        "hypnogram":
            "443432222211222333321112222222222111133333322221112233333333332232222334",
        "sleepScore": 58,
        "sleepDate": "2023-10-21"
      }
    },
    {
      "id": 3,
      "userId": "2442bede-d781-49d7-99fa-9025c5b35b15",
      "createdAt": "2023-10-22T21:24:59.395986",
      "smarterSleepScore": 76,
      "survey": {
        "id": 3,
        "createdAt": "2023-10-22T21:24:59.396077",
        "sleepQuality": 3,
        "wakePreference": 3,
        "temperaturePreference": 2,
        "lightsDisturbance": true,
        "sleepEarlier": true,
        "sleepDuration": 350,
        "surveyDate": "2023-10-22"
      },
      "wearableLog": {
        "id": 3,
        "sleepStart": "2023-10-22T21:24:59.396289",
        "sleepEnd": "2023-10-23T05:24:59.396312",
        "hypnogram":
            "443432222211222333321112222222222111133333322221112233333333332232222334",
        "sleepScore": 75,
        "sleepDate": "2023-10-22"
      }
    },
    {
      "id": 4,
      "userId": "2442bede-d781-49d7-99fa-9025c5b35b15",
      "createdAt": "2023-10-23T21:24:59.395986",
      "smarterSleepScore": 67,
      "survey": {
        "id": 4,
        "createdAt": "2023-10-23T21:24:59.396077",
        "sleepQuality": 2,
        "wakePreference": 0,
        "temperaturePreference": 0,
        "lightsDisturbance": false,
        "sleepEarlier": false,
        "sleepDuration": 450,
        "surveyDate": "2023-10-23"
      },
      "wearableLog": {
        "id": 4,
        "sleepStart": "2023-10-23T21:24:59.396289",
        "sleepEnd": "2023-10-24T05:24:59.396312",
        "hypnogram":
            "443432222211222333321112222222222111133333322221112233333333332232222334",
        "sleepScore": 64,
        "sleepDate": "2023-10-23"
      }
    },
    {
      "id": 5,
      "userId": "2442bede-d781-49d7-99fa-9025c5b35b15",
      "createdAt": "2023-10-24T21:24:59.395986",
      "smarterSleepScore": 89,
      "survey": {
        "id": 5,
        "createdAt": "2023-10-24T21:24:59.396077",
        "sleepQuality": 4,
        "wakePreference": 1,
        "temperaturePreference": 0,
        "lightsDisturbance": false,
        "sleepEarlier": true,
        "sleepDuration": 350,
        "surveyDate": "2023-10-24"
      },
      "wearableLog": {
        "id": 5,
        "sleepStart": "2023-10-24T21:24:59.396289",
        "sleepEnd": "2023-10-25T05:24:59.396312",
        "hypnogram":
            "443432222211222333321112222222222111133333322221112233333333332232222334",
        "sleepScore": 85,
        "sleepDate": "2023-10-24"
      }
    },
    {
      "id": 6,
      "userId": "2442bede-d781-49d7-99fa-9025c5b35b15",
      "createdAt": "2023-10-25T21:24:59.395986",
      "smarterSleepScore": 15,
      "survey": {
        "id": 6,
        "createdAt": "2023-10-25T21:24:59.396077",
        "sleepQuality": 3,
        "wakePreference": 2,
        "temperaturePreference": 1,
        "lightsDisturbance": false,
        "sleepEarlier": true,
        "sleepDuration": 51,
        "surveyDate": "2023-10-25"
      },
      "wearableLog": {
        "id": 6,
        "sleepStart": "2023-10-25T21:24:59.396289",
        "sleepEnd": "2023-10-26T05:24:59.396312",
        "hypnogram":
            "443432222211222333321112222222222111133333322221112233333333332232222334",
        "sleepScore": 21,
        "sleepDate": "2023-10-25"
      }
    },
    {
      "id": 7,
      "userId": "2442bede-d781-49d7-99fa-9025c5b35b15",
      "createdAt": "2023-10-26T21:24:59.395986",
      "smarterSleepScore": 95,
      "survey": {
        "id": 7,
        "createdAt": "2023-10-26T21:24:59.396077",
        "sleepQuality": 4,
        "wakePreference": 3,
        "temperaturePreference": 2,
        "lightsDisturbance": true,
        "sleepEarlier": false,
        "sleepDuration": 330,
        "surveyDate": "2023-10-26"
      },
      "wearableLog": {
        "id": 7,
        "sleepStart": "2023-10-26T21:24:59.396289",
        "sleepEnd": "2023-10-27T05:24:59.396312",
        "hypnogram":
            "443432222211222333321112222222222111133333322221112233333333332232222334",
        "sleepScore": 95,
        "sleepDate": "2023-10-26"
      }
    },
    {
      "id": 8,
      "userId": "2442bede-d781-49d7-99fa-9025c5b35b15",
      "createdAt": "2023-10-27T21:24:59.395986",
      "smarterSleepScore": 84,
      "survey": {
        "id": 8,
        "createdAt": "2023-10-27T21:24:59.396077",
        "sleepQuality": 5,
        "wakePreference": 2,
        "temperaturePreference": 1,
        "lightsDisturbance": false,
        "sleepEarlier": true,
        "sleepDuration": 360,
        "surveyDate": "2023-10-27"
      },
      "wearableLog": {
        "id": 8,
        "sleepStart": "2023-10-27T21:24:59.396289",
        "sleepEnd": "2023-10-28T05:24:59.396312",
        "hypnogram":
            "443432222211222333321112222222222111133333322221112233333333332232222334",
        "sleepScore": 84,
        "sleepDate": "2023-10-27"
      }
    },
    {
      "id": 9,
      "userId": "2442bede-d781-49d7-99fa-9025c5b35b15",
      "createdAt": "2023-10-28T21:24:59.395986",
      "smarterSleepScore": 98,
      "survey": {
        "id": 9,
        "createdAt": "2023-10-28T21:24:59.396077",
        "sleepQuality": 3,
        "wakePreference": 1,
        "temperaturePreference": 0,
        "lightsDisturbance": true,
        "sleepEarlier": false,
        "sleepDuration": 400,
        "surveyDate": "2023-10-28"
      },
      "wearableLog": {
        "id": 9,
        "sleepStart": "2023-10-28T21:24:59.396289",
        "sleepEnd": "2023-10-29T05:24:59.396312",
        "hypnogram":
            "443432222211222333321112222222222111133333322221112233333333332232222334",
        "sleepScore": 98,
        "sleepDate": "2023-10-28"
      }
    },
    {
      "id": 10,
      "userId": "2442bede-d781-49d7-99fa-9025c5b35b15",
      "createdAt": "2023-10-29T21:24:59.395986",
      "smarterSleepScore": 100,
      "survey": {
        "id": 10,
        "createdAt": "2023-10-29T21:24:59.396077",
        "sleepQuality": 4,
        "wakePreference": 0,
        "temperaturePreference": 0,
        "lightsDisturbance": false,
        "sleepEarlier": true,
        "sleepDuration": 390,
        "surveyDate": "2023-10-29"
      },
      "wearableLog": {
        "id": 10,
        "sleepStart": "2023-10-29T21:24:59.396289",
        "sleepEnd": "2023-10-30T05:24:59.396312",
        "hypnogram":
            "443432222211222333321112222222222111133333322221112233333333332232222334",
        "sleepScore": 100,
        "sleepDate": "2023-10-29"
      }
    },
  ];

  IconData getSleepIcon(int sleepDuration) {
    if (sleepDuration < 240) {
      return Icons.sentiment_very_dissatisfied;
    } else if (sleepDuration >= 240 && sleepDuration <= 420) {
      return Icons.sentiment_satisfied;
    } else if (sleepDuration > 420 && sleepDuration <= 540) {
      return Icons.sentiment_very_satisfied;
    } else {
      return Icons.sentiment_dissatisfied;
    }
  }

  Color getSleepScoreColor(int score) {
    if (score >= 0 && score <= 100) {
      final colorTween = ColorTween(
        begin: Colors.red,
        end: Colors.green,
      );
      return colorTween.transform(score / 100)!;
    } else {
      return Colors.black;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Sleep Review'),
      ),
      body: ListView.builder(
        itemCount: sleepData.length,
        itemBuilder: (context, index) {
          final sleepEntry = sleepData[index];
          final sleepScore = sleepEntry["smarterSleepScore"];
          final sleepDuration = sleepEntry["survey"]["sleepDuration"];
          final sleepScoreColor = getSleepScoreColor(sleepScore);
          final sleepIcon = getSleepIcon(sleepDuration);

          return Card(
            margin: EdgeInsets.all(8.0),
            elevation: 4,
            child: Container(
              padding: EdgeInsets.all(16),
              child: ListTile(
                title:
                    Text('Sleep Date: ${sleepEntry["survey"]["surveyDate"]}'),
                leading: Icon(sleepIcon, color: sleepScoreColor, size: 36),
                trailing: Stack(
                  alignment: Alignment.center,
                  children: <Widget>[
                    Transform.scale(
                      scale: 0.8,
                      child: CircularProgressIndicator(
                        value: sleepScore / 100,
                        valueColor:
                            AlwaysStoppedAnimation<Color>(sleepScoreColor),
                      ),
                    ),
                    Text('$sleepScore',
                        style: TextStyle(fontSize: 14, color: sleepScoreColor)),
                  ],
                ),
                onTap: () {
                  // Add your onTap action
                },
              ),
            ),
          );
        },
      ),
    );
  }
}
