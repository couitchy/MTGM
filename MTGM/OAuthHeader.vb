Imports System.Text
Imports System.Security.Cryptography
Public Class OAuthHeader
    Private VmAppToken As String = "SNHYNcu9fbXAORqY"                       'Clé publique
    Private VmAppSecret As String = "66rhX6mLKuoRHbNV4vJlz5JFv7Sgn3aP"      'Clé privée
    Private VmAccessToken As String = ""                                    'Non utilisé
    Private VmAccessSecret As String = ""                                   'Non utilisé
    Private VmSignatureMethod As String = "HMAC-SHA1"                       'Méthode de hashage
    Private VmVersion As String = "1.0"                                     'Version OAuth
    Private VmHeaderParams As IDictionary(Of String, String)                'En-têtes pour la requête HTTP
    Public Sub New
    '------------------------------------------------
    'Construction d'une authentification OAuth unique
    '------------------------------------------------
    Dim VpNonce As String = Guid.NewGuid.ToString("n")
    Dim VpTimestamp As String = CInt((DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1))).TotalSeconds).ToString
        VmHeaderParams = New Dictionary(Of String, String)
        VmHeaderParams.Add("oauth_consumer_key", VmAppToken)
        VmHeaderParams.Add("oauth_token", VmAccessToken)
        VmHeaderParams.Add("oauth_nonce", VpNonce)
        VmHeaderParams.Add("oauth_timestamp", VpTimestamp)
        VmHeaderParams.Add("oauth_signature_method", VmSignatureMethod)
        VmHeaderParams.Add("oauth_version", VmVersion)
    End Sub
    Public Function GetAuthorizationHeader(VpMethod As String, VpURL As String) As String
    '--------------------------
    'Calcul des en-têtes signés
    '--------------------------
    Dim VpBaseString As String
    Dim VpEncodedParams As New SortedDictionary(Of String, String)
    Dim VpParamStrings As New List(Of String)
    Dim VpParamString As String
    Dim VpSignatureKey As String
    Dim VpHasher As HMAC
    Dim VpOAuthSignature As String
    Dim VpHeaderParamStrings As New List(Of String)
        'Add the realm parameter to the header params
        VmHeaderParams.Add("realm", VpURL)
        'Start composing the base string from the method and request URI
        VpBaseString = VpMethod.ToUpper + "&" + Uri.EscapeDataString(VpURL) + "&"
        'Gather, encode, and sort the base string parameters
        For Each VpParameter As KeyValuePair(Of String, String) In VmHeaderParams
            If Not VpParameter.Key.Equals("realm") Then
                VpEncodedParams.Add(Uri.EscapeDataString(VpParameter.Key), Uri.EscapeDataString(VpParameter.Value))
            End If
        Next VpParameter
        'Expand the base string by the encoded parameter=value pairs
        For Each VpParameter As KeyValuePair(Of String, String) In VpEncodedParams
            VpParamStrings.Add(VpParameter.Key + "=" + VpParameter.Value)
        Next VpParameter
        VpParamString = Uri.EscapeDataString(String.Join("&", VpParamStrings.ToArray))
        VpBaseString += VpParamString
        'Create the OAuth signature
        VpSignatureKey = Uri.EscapeDataString(VmAppSecret) + "&" + Uri.EscapeDataString(VmAccessSecret)
        VpHasher = HMACSHA1.Create
        VpHasher.Key = Encoding.UTF8.GetBytes(VpSignatureKey)
        VpOAuthSignature = Convert.ToBase64String(VpHasher.ComputeHash(Encoding.UTF8.GetBytes(VpBaseString)))
        'Include the OAuth signature parameter in the header parameters array
        VmHeaderParams.Add("oauth_signature", VpOAuthSignature)
        'Construct the header string
        For Each VpParameter As KeyValuePair(Of String, String) In VmHeaderParams
            VpHeaderParamStrings.Add(VpParameter.Key + "=""" + VpParameter.Value + """")
        Next VpParameter
        Return "OAuth " + String.Join(", ", VpHeaderParamStrings.ToArray)
    End Function
End Class
