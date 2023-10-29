import 'dart:convert';

class DeviceSchedule {
  final int id;
  final int deviceId;
  final int sleepSettingId;
  final DateTime scheduledTime;
  final Map<String, dynamic> settings;

  DeviceSchedule(this.id, this.deviceId, this.sleepSettingId,
      this.scheduledTime, this.settings);

  factory DeviceSchedule.fromJson(Map<String, dynamic> json) {
    return DeviceSchedule(
      json['id'] as int,
      json['deviceId'] as int,
      json['sleepSettingId'] as int,
      DateTime.parse(json['scheduledTime'] as String),
      _decodeSettings(json['settings']),
    );
  }

  Map<String, dynamic> toJson() => <String, dynamic>{
        'id': id,
        'deviceId': deviceId,
        'sleepSettingId': sleepSettingId,
        'scheduledTime': scheduledTime.toIso8601String(),
        'settings': settings,
      };

  static Map<String, dynamic> _decodeSettings(String jsonString) {
    return json.decode(jsonString);
  }
}
