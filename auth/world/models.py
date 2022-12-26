from django.db import models


class Character(models.Model):

    name = models.CharField(
        verbose_name="Name",
        max_length=32
    )

    color = models.CharField(
        verbose_name="Color",
        max_length=10,
    )

    class Meta:
        verbose_name = "Character"
        verbose_name_plural = "Characteres"
