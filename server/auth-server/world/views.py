from rest_framework.generics import CreateAPIView

from account.permissions import IsServerAuthenticated

from .models import Character
from .serializers import CharacterCreationSerializer


class CharacterCreationView(CreateAPIView):
    model = Character
    serializer_class = CharacterCreationSerializer
    permission_classes = (IsServerAuthenticated, )

character_create = CharacterCreationView.as_view()
