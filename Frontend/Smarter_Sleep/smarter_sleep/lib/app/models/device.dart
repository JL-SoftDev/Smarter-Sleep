import 'package:json_annotation/json_annotation.dart';
part 'device.g.dart';

@JsonSerializable()
class Device {
  int id = 0;
  String name = "";
  String type = "";
  String ip = "";
  int port = 0;
  String status = "";

  Device();

  factory Device.fromJson(Map<String, dynamic> json) => _$DeviceFromJson(json);
  Map<String, dynamic> toJson() => _$DeviceToJson(this);
}
