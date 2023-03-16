from django.db import models


class Character(models.Model):

    token = models.OneToOneField(
        "account.Token",
        on_delete=models.CASCADE,
        related_name="character"
    )

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


class Inventory(models.Model):

    character = models.ForeignKey(
        Character,
        on_delete=models.CASCADE
    )

    itemId = models.PositiveIntegerField(
        verbose_name="Item Name"
    )

    amount = models.PositiveIntegerField(
        verbose_name="Amount"
    )

    source = models.CharField(
        verbose_name="Source",
        max_length=32
    )

    class Meta:
        verbose_name = "Inventory"


class Equipment(models.Model):

    character = models.ForeignKey(
        Character,
        on_delete=models.CASCADE
    )

    item_id = models.PositiveIntegerField(
        verbose_name="Item ID"
    )

    item_slot = models.PositiveSmallIntegerField(
        verbose_name="Slot"
    )

    class Meta:
        verbose_name = "Equipment"
        unique_together = (('character', 'item_slot'), )
