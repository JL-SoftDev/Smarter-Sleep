import 'package:flutter/material.dart';
import 'package:json_annotation/json_annotation.dart';

@JsonSerializable()
class Device {
  int? id;
  String userId;
  String name;
  String type;
  String? ip;
  int? port;
  String? status;

  Device({
    this.id,
    required this.userId,
    required this.name,
    required this.type,
    this.ip,
    this.port,
    this.status,
  });

  factory Device.fromJson(Map<String, dynamic> json) {
    return Device(
        id: json['id'],
        userId: json['userId'],
        name: json['name'],
        type: json['type'],
        ip: json['ip'],
        port: json['port'],
        status: json['status']);
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = {
      'userId': userId,
      'name': name,
      'type': type,
      'ip': ip,
      'port': port,
      'status': status,
    };

    if (id != null) {
      data['id'] = id;
    }

    return data;
  }

  IconData getDeviceIcon() {
    switch (type) {
      case 'alarm':
        return Icons.alarm;
      case 'light':
        return Icons.lightbulb;
      case 'thermostat':
        return Icons.thermostat;
      default:
        return Icons.device_unknown;
    }
  }
}
