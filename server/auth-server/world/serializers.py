from rest_framework import serializers

from account.models import Token
from .models import Character


class CharacterCreationSerializer(serializers.ModelSerializer):
    
    token = serializers.PrimaryKeyRelatedField(queryset=Token.objects.all())

    class Meta:
        model = Character
        fields = ("token", "name", "color", )

    def validate_token_id(self, value):
        request = self.context["request"]

        char_queryset = Character.objects.filter(token=value)

        if value.owner != request.user_server.address:
            raise serializers.ValidationError("Invalid owner")

        if char_queryset.exists():
            raise serializers.ValidationError("Character exists.")
      
        return value

    def create(self, validated_data):
        return Character.objects.create(**validated_data)
