import 'dart:convert';

import 'package:smarter_sleep/app/models/device.dart';

class DeviceSchedule {
  int? id;
  int deviceId;
  Device? device;
  int? sleepSettingId;
  final DateTime scheduledTime;
  final Map<String, dynamic>? settings;
  bool? userModified = false;

  DeviceSchedule({
    this.id,
    required this.deviceId,
    this.device,
    required this.sleepSettingId,
    required this.scheduledTime,
    this.settings,
    this.userModified,
  });

  factory DeviceSchedule.fromJson(Map<String, dynamic> json) {
    Device? jsonDevice;
    int? jsonId;
    if (json['device'] != null) {
      jsonDevice = Device.fromJson(json['device']);
      if (jsonDevice.id != null) {
        jsonId = jsonDevice.id!;
      }
    }
    if (json['deviceId'] != null) {
      jsonId = json['deviceId'];
    }

    return DeviceSchedule(
      id: json['id'],
      deviceId: jsonId!,
      device: jsonDevice,
      sleepSettingId: json['sleepSettingId'],
      scheduledTime: DateTime.parse(json['scheduledTime']),
      settings: _decodeSettings(json['settings']),
      userModified: json['userModified'],
    );
  }

  Map<String, dynamic> toJson() => <String, dynamic>{
        'id': id,
        'deviceId': deviceId,
        'sleepSettingId': sleepSettingId,
        'scheduledTime': scheduledTime.toIso8601String(),
        'settings': jsonEncode(settings),
        'userModified': userModified,
      };

  static Map<String, dynamic>? _decodeSettings(String? jsonString) {
    if (jsonString != null) {
      return json.decode(jsonString);
    } else {
      return null;
    }
  }
}
