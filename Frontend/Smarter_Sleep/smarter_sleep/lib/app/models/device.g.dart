// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'device.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Device _$DeviceFromJson(Map<String, dynamic> json) => Device()
  ..id = json['id'] as int
  ..userId = json['userId'] as String
  ..name = json['name'] as String
  ..type = json['type'] as String
  ..ip = json['ip'] as String?
  ..port = json['port'] as int
  ..status = json['status'] as String?;

Map<String, dynamic> _$DeviceToJson(Device instance) => <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'name': instance.name,
      'type': instance.type,
      'ip': instance.ip,
      'port': instance.port,
      'status': instance.status,
    };
