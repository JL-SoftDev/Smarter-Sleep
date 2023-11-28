import 'package:json_annotation/json_annotation.dart';

@JsonSerializable()
class Device {
  int? id;
  String userId = "";
  String name = "";
  String type = "";
  String? ip = "";
  int? port = 0;
  String? status = "";

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

  Map<String, dynamic> toJson() => <String, dynamic>{
        'id': id,
        'userId': userId,
        'name': name,
        'type': type,
        'ip': ip,
        'port': port,
        'status': status,
      };
}
