import 'package:smarter_sleep/app/models/sleep_schedule.dart';

import 'survey.dart';
import 'wearable_log.dart';

class SleepReview {
  final int id;
  final String userId;
  final DateTime createdAt;
  final int smarterSleepScore;
  final Survey survey;
  final WearableLog wearableLog;
  final SleepSchedule? sleepSchedule;

  SleepReview({
    required this.id,
    required this.userId,
    required this.createdAt,
    required this.smarterSleepScore,
    required this.survey,
    required this.wearableLog,
    this.sleepSchedule,
  });

  factory SleepReview.fromJson(Map<String, dynamic> json) {
    return SleepReview(
        id: json['id'],
        userId: json['userId'],
        createdAt: DateTime.parse(json['createdAt']),
        smarterSleepScore: json['smarterSleepScore'],
        survey: Survey.fromJson(json['survey']),
        wearableLog: WearableLog.fromJson(json['wearableLog']),
        sleepSchedule: _parseSleepSchedule(json['sleepSetting']));
  }

  static SleepSchedule? _parseSleepSchedule(dynamic json) {
    if (json != null) {
      return SleepSchedule.fromJson(json);
    }
    return null;
  }
}
