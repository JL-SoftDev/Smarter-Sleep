import 'survey.dart';
import 'wearable_log.dart';

class SleepReview {
  final int id;
  final String userId;
  final DateTime createdAt;
  final int smarterSleepScore;
  final Survey survey;
  final WearableLog wearableLog;

  SleepReview({
    required this.id,
    required this.userId,
    required this.createdAt,
    required this.smarterSleepScore,
    required this.survey,
    required this.wearableLog,
  });

  factory SleepReview.fromJson(Map<String, dynamic> json) {
    return SleepReview(
      id: json['id'],
      userId: json['userId'],
      createdAt: DateTime.parse(json['createdAt']),
      smarterSleepScore: json['smarterSleepScore'],
      survey: Survey.fromJson(json['survey']),
      wearableLog: WearableLog.fromJson(json['wearableLog']),
    );
  }
}
