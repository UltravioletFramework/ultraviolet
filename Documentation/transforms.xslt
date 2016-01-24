<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="dprop">
	<h3>Dependency Property Information</h3>
	<table class="dpropTable">
		<tr>
			<td>Identifier field</td>
			<td>
				<xsl:copy-of select="dpropField/node()"/>
			</td>
		</tr>
		<tr>
			<td>Styling name</td>
			<td>
				<xsl:copy-of select="dpropStylingName/node()"/>
			</td>
		</tr>
		<tr>
			<td>Metadata properties set to <see langword="true"/></td>
			<td>
				<xsl:copy-of select="dpropMetadata/node()"/>
			</td>
		</tr>
	</table>
  </xsl:template>

  <xsl:template match="revt">
    <h3>Routed Event Information</h3>
    <table class="revtTable">
      <tr>
        <td>Identifier field</td>
        <td>
          <xsl:copy-of select="revtField/node()"/>
        </td>
      </tr>
	  <tr>
		<td>Styling name</td>
		<td>
		  <xsl:copy-of select="revtStylingName/node()"/>
		</td>
	  </tr>
      <tr>
        <td>Routing strategy</td>
        <td>
          <xsl:copy-of select="revtStrategy/node()"/>
        </td>
      </tr>
      <tr>
        <td>Delegate</td>
        <td>
          <xsl:copy-of select="revtDelegate/node()"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()"/>
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>