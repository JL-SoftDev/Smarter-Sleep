import 'package:smarter_sleep/app/models/device_schedule.dart';

class SleepSchedule {
  final String userId;
  final DateTime scheduledSleep;
  final DateTime scheduledWake;
  String? scheduledHypnogram;
  final List<DeviceSchedule>? deviceSchedules;

  SleepSchedule({
    required this.userId,
    required this.scheduledSleep,
    required this.scheduledWake,
    this.scheduledHypnogram,
    this.deviceSchedules,
  });

  factory SleepSchedule.fromJson(Map<String, dynamic> json) {
    return SleepSchedule(
      userId: json['userId'],
      scheduledSleep: DateTime.parse(json['scheduledSleep']),
      scheduledWake: DateTime.parse(json['scheduledWake']),
      scheduledHypnogram: json['scheduledHypnogram'],
      deviceSchedules: _parseDeviceSchedules(json['deviceSettings']),
    );
  }

  Map<String, dynamic> toJson() => <String, dynamic>{
        'userId': userId,
        'scheduledSleep': scheduledSleep.toIso8601String(),
        'scheduledWake': scheduledWake.toIso8601String(),
        'scheduledHypnogram': scheduledHypnogram,
        'deviceSchedules': deviceSchedules
            ?.map((deviceSchedule) => deviceSchedule.toJson())
            .toList(),
      };

  static List<DeviceSchedule>? _parseDeviceSchedules(dynamic json) {
    if (json != null) {
      return List<DeviceSchedule>.from(
        (json as List).map(
          (deviceScheduleJson) => DeviceSchedule.fromJson(deviceScheduleJson),
        ),
      );
    }
    return null;
  }
}
