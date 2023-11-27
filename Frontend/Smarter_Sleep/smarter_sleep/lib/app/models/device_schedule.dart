import 'dart:convert';

class DeviceSchedule {
  final int id;
  final int deviceId;
  final int sleepSettingId;
  final DateTime scheduledTime;
  final Map<String, dynamic>? settings;

  DeviceSchedule({
    required this.id,
    required this.deviceId,
    required this.sleepSettingId,
    required this.scheduledTime,
    this.settings,
  });

  factory DeviceSchedule.fromJson(Map<String, dynamic> json) {
    return DeviceSchedule(
      id: json['id'],
      deviceId: json['deviceId'],
      sleepSettingId: json['sleepSettingId'],
      scheduledTime: DateTime.parse(json['scheduledTime']),
      settings: _decodeSettings(json['settings']),
    );
  }
  static Map<String, dynamic>? _decodeSettings(String? jsonString) {
    if (jsonString != null) {
      return json.decode(jsonString);
    } else {
      return null;
    }
  }
}
