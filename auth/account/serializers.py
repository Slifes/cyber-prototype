import string
import secrets

from web3 import Web3

from django.utils import timezone
from django.utils.translation import gettext_lazy as _
from rest_framework import serializers

from .utils import validate_eth_address, recover_to_addr
from .models import Token, Authentication, SessionMap


class CharacterSerializer(serializers.ModelSerializer):

    class Meta:
        model = Token
        fields = ('address', 'token_id', )


class AuthenticationSerializer(serializers.ModelSerializer):

    address = serializers.CharField()
    signature = serializers.CharField(write_only=True)
    token = serializers.CharField(read_only=True)
    expire_at = serializers.CharField(read_only=True)

    class Meta:
        model = Authentication
        fields = ('address', 'signature', 'token', 'expire_at', )

    def validate_signature(self, sig):
        if any([
            len(sig) != 132,
            sig[130:] != '1b' and sig[130:] != '1c',
            not all(c in string.hexdigits for c in sig[2:])
        ]):
            raise serializers.ValidationError(_('Invalid signature encode'))
        return sig

    def validate_address(self, value):
        if validate_eth_address(value):
            raise serializers.ValidationError(_("Invalid address"))

        return value

    def validate(self, attrs):
        address = attrs['address']
        signature = attrs['signature']
        msg = Web3.toHex(text='1')

        print("address: " + address)
        print("signature: " + signature)
        print(recover_to_addr(msg, signature))

        if address != recover_to_addr(msg, signature):
            raise serializers.ValidationError(_("Invalid signature"))

        return attrs

    def create(self, validated_data):
        return Authentication.objects.create(
            address=validated_data['address'],
            token=secrets.token_urlsafe(40),
            expire_at=timezone.now() + timezone.timedelta(minutes=10)
        )


class SessionMapSerializer(serializers.ModelSerializer):

    character = serializers.PrimaryKeyRelatedField(queryset=Token.objects.all(), write_only=True)
    auth_token = serializers.CharField(read_only=True)
    expire_at = serializers.DateTimeField(read_only=True)

    class Meta:
        model = SessionMap
        fields = ('character', 'auth_token', 'expire_at')

    def create(self, validated_data):
        # return SessionMap.objects.create(
        #     character=validated_data['character'],
        #     auth_token=
        # )
        pass
