from django.db import models


class Token(models.Model):

    owner = models.CharField(
        verbose_name="Address",
        max_length=64,
        db_index=True
    )

    token_id = models.PositiveIntegerField(
        verbose_name="TokenID",
        unique=True,
        db_index=True
    )

    class Meta:
        verbose_name = "Token"
        verbose_name_plural = "Tokens"


class Authentication(models.Model):

    address = models.CharField(
        verbose_name="Address",
        max_length=64,
        db_index=True
    )

    token = models.CharField(
        verbose_name="Token",
        max_length=100
    )

    expire_at = models.DateTimeField(
        verbose_name="Expire at",
    )

    valid = models.BooleanField(
        verbose_name="Is Valid?",
        default=True
    )

    class Meta:
        verbose_name = "Authentication"
        verbose_name_plural = "Authentications"


class SessionMap(models.Model):

    address = models.CharField(
        verbose_name="Address",
        max_length=64,
        db_index=True
    )

    character = models.ForeignKey(
        Token,
        on_delete=models.CASCADE
    )

    auth_token = models.CharField(
        verbose_name="Token",
        max_length=100,
    )

    expire_at = models.DateTimeField(
        verbose_name="Expire at",
    )

    authenticated = models.BooleanField(
        verbose_name="Authenticated",
        default=False
    )

    class Meta:
        verbose_name = "Session"
        verbose_name_plural = "Sessions"
